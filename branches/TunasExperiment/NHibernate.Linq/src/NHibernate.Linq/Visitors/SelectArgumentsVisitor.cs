using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Linq.Expressions;
using NHibernate.Linq.Transform;
using NHibernate.Linq.Util;
using NHibernate.Transform;
using NHibernate.Type;
using Expression = System.Linq.Expressions.Expression;
using NHProjections = NHibernate.Criterion.Projections;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Provides the appropriate NHibernate selection projections and/or IResultTransformers
    /// based on a given expression tree.
    /// </summary>
	public class SelectArgumentsVisitor : NHibernateExpressionVisitor
    {
        #region Fields & Properties

        private static readonly ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
		private static readonly ISQLFunction arithmaticDivide = new VarArgsSQLFunction("(", "/", ")");
		private static readonly ISQLFunction arithmaticMultiply = new VarArgsSQLFunction("(", "*", ")");
		private static readonly ISQLFunction arithmaticSubstract = new VarArgsSQLFunction("(", "-", ")");

        private readonly ICriteria _rootCriteria;
        private readonly ISession _session;
        private readonly List<IProjection> _projections;
        private IResultTransformer _transformer;
        private ICriteriaQuery _criteriaQuery;

        public IProjection Projection
        {
            get
            {
                if (_projections.Count == 0)
                    return null;

                if (_projections.Count == 1)
                    return _projections[0];

                ProjectionList list = NHProjections.ProjectionList();
                foreach (var projection in _projections)
                    list.Add(projection);

                return list;
            }
        }

        public IResultTransformer Transformer
        {
            get { return _transformer; }
        }

        private ICriteriaQuery CriteriaQuery
        {
            get
            {
                if (_criteriaQuery == null)
                    _criteriaQuery = _rootCriteria.GenerateCriteriaQuery(_session.SessionFactory, _rootCriteria.GetEntityOrClassName());
                return _criteriaQuery;
            }
        }

        #endregion

        public SelectArgumentsVisitor(ICriteria rootCriteria, ISession session)
		{
            _rootCriteria = rootCriteria;
            _session = session;
            _projections = new List<IProjection>();
		}

        protected override Expression VisitMethodCall(MethodCallExpression expr)
        {
        	IMethodTranslator tr = MethodTranslatorRegistry.Current.GetTranslatorInstanceForMethod(expr.Method);
			tr.Initialize(this._session,this._rootCriteria);
        	this._projections.Add(tr.GetProjection(expr));
        	return expr;
        }

        protected override Expression VisitConstant(ConstantExpression expr)
		{
			_projections.Add(new ConstantProjection(expr.Value));
            return expr;
		}

        protected override NewExpression VisitNew(NewExpression expr)
        {
            NewExpression newExpr = base.VisitNew(expr);
            _transformer = new TypeSafeConstructorMemberInitResultTransformer(expr);
            return newExpr;
        }

        protected override Expression VisitMemberInit(MemberInitExpression expr)
        {
            Expression newExpr = base.VisitMemberInit(expr);
			_transformer = new TypeSafeConstructorMemberInitResultTransformer(expr);
            return newExpr;
        }

        protected override Expression VisitConditional(ConditionalExpression expr)
        {
            var visitorTrue = new SelectArgumentsVisitor(_rootCriteria, _session);
            visitorTrue.Visit(expr.IfTrue);

            var visitorFalse = new SelectArgumentsVisitor(_rootCriteria, _session);
            visitorFalse.Visit(expr.IfFalse);

            var visitorCondition = new WhereArgumentsVisitor(_rootCriteria, _session);
            visitorCondition.Visit(expr.Test);
            Conjunction conjunction = NHibernate.Criterion.Expression.Conjunction();
            foreach (var criterion in visitorCondition.CurrentCriterions)
            {
                conjunction.Add(criterion);
            }

            _projections.Add(
                NHibernate.Criterion.Projections
                    .Conditional(conjunction,
                        visitorTrue.Projection,
                        visitorFalse.Projection)
                        );

            return expr;
        }

        protected override Expression VisitBinary(BinaryExpression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Add:
                    VisitAddExpression(expr);
                    break;
                case ExpressionType.Divide:
                    VisitDivideExpression(expr);
                    break;
                case ExpressionType.Multiply:
                    VisitMultiplyExpression(expr);
                    break;
                case ExpressionType.Subtract:
                    VisitSubtractExpression(expr);
                    break;
                default:
            		VisitCriterionExpression(expr);
                    return expr;
            }

            return expr;
        }

        #region Arithmetic

        private void VisitAddExpression(BinaryExpression expr)
		{
			var leftVisitor = new SelectArgumentsVisitor(_rootCriteria, _session);
			var rightVisitor = new SelectArgumentsVisitor(_rootCriteria, _session);
			leftVisitor.Visit(expr.Left);
			rightVisitor.Visit(expr.Right);

			var joinedProjections = new List<IProjection>();
			joinedProjections.AddRange(leftVisitor._projections);
			joinedProjections.AddRange(rightVisitor._projections);

			IType[] types = joinedProjections[0].GetTypes(_rootCriteria, CriteriaQuery);
			var useConcat = types[0] is AbstractStringType;
			SqlFunctionProjection projection;
			if (useConcat)
			{
				projection = new SqlFunctionProjection("concat", types[0], joinedProjections.ToArray());
			}
			else
			{
				projection = new SqlFunctionProjection(arithmaticAddition, types[0], joinedProjections.ToArray());
			}
			_projections.Add(projection);
		}

        private void VisitMultiplyExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticMultiply);
		}

        private void VisitSubtractExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticSubstract);
		}

        private void VisitDivideExpression(BinaryExpression expr)
		{
			VisitAritmaticOperation(expr, arithmaticDivide);
		}

		private void VisitAritmaticOperation(BinaryExpression expr, ISQLFunction arithmaticOperation)
		{
			var leftVisitor = new SelectArgumentsVisitor(_rootCriteria, _session);
			var rightVisitor = new SelectArgumentsVisitor(_rootCriteria, _session);
			leftVisitor.Visit(expr.Left);
			rightVisitor.Visit(expr.Right);

			var joinedProjections = new List<IProjection>();
			joinedProjections.AddRange(leftVisitor._projections);
			joinedProjections.AddRange(rightVisitor._projections);
			var types = joinedProjections[0].GetTypes(_rootCriteria, CriteriaQuery);
			var projection = new SqlFunctionProjection(arithmaticOperation, types[0], joinedProjections.ToArray());
			_projections.Add(projection);
        }

        #endregion
		private Expression VisitCriterionExpression(Expression expr)
		{
			IEnumerable<ICriterion> criterion = WhereArgumentsVisitor.GetCriterion(_rootCriteria, _session, expr);

			if (criterion.Count() > 0)
			{
				Conjunction conjunction = new Conjunction();
				foreach (ICriterion crit in criterion)
				{
					conjunction.Add(crit);
				}

				_projections.Add(Projections.Conditional(
					conjunction,
					Projections.Constant(true),
					Projections.Constant(false))
				);
			}

			return expr;
		}
        protected override Expression VisitUnary(UnaryExpression expr)
        {
            if (expr.NodeType == ExpressionType.Convert)
            {
                var visitor = new SelectArgumentsVisitor(_rootCriteria, _session);
                visitor.Visit(expr.Operand);

                ProjectionList list = NHProjections.ProjectionList();
                foreach (IProjection proj in visitor._projections)
                    list.Add(proj);

                var projection = new CastProjection(NHibernateUtil.GuessType(expr.Type), list);
                _projections.Add(projection);
            }

            return expr;
        }

        protected override Expression VisitEntity(EntityExpression expr)
        {
            if (_rootCriteria.GetCriteriaByAlias(expr.Alias) != null)
            {
                _transformer = new LinqJoinResultsTransformer(expr.Type);
            }

            return expr;
        }

        protected override Expression VisitMemberAccess(MemberExpression expr)
        {
            //this must be a grouping parameter member access
            IProjection projection = _rootCriteria.GetProjection();
            if (projection != null && projection.IsGrouped)
            {
                _projections.Add(NHProjections.Alias(projection, expr.Member.Name));
            }

            return expr;
        }

        protected override Expression VisitPropertyAccess(PropertyAccessExpression expr)
        {
            string memberName = MemberNameVisitor.GetMemberName(_rootCriteria, expr);
            _projections.Add(NHProjections.Property(memberName));
            return expr;
        }
	}
}
