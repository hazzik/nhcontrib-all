﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NHibernate.Envers.Query;
using Iesi.Collections;

namespace NHibernate.Envers.Tools.Query
{
/**
 * Generates metadata for to-one relations (reference-valued properties).
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */
public class Parameters {
    public static String AND = "and";
    public static String OR = "or";
    
    /**
     * Main alias of the entity.
     */
    private String alias;
    /**
     * Connective between these parameters - "and" or "or".
     */
    private String connective;
    /**
     * For use by the parameter generator. Must be the same in all "child" (and parent) parameters.
     */
    private MutableInteger queryParamCounter;

    /**
     * A list of sub-parameters (parameters with a different connective).
     */
    private  IList<Parameters> subParameters;
    /**
     * A list of negated parameters.
     */
    private IList<Parameters> negatedParameters;
    /**
     * A list of complete where-expressions.
     */
    private IList<String> expressions;
    /**
     * Values of parameters used in expressions.
     */
    private IDictionary<String, Object> localQueryParamValues;

    Parameters(String alias, String connective, MutableInteger queryParamCounter) {
        this.alias = alias;
        this.connective = connective;
        this.queryParamCounter = queryParamCounter;

        subParameters = new List<Parameters>();   
        negatedParameters = new List<Parameters>();
        expressions = new List<String>();
        localQueryParamValues = new Dictionary<String, Object>(); 
    }

    private String generateQueryParam() {
        return "_p" + queryParamCounter.getAndIncrease();
    }

    /**
     * Adds sub-parameters with a new connective. That is, the parameters will be grouped in parentheses in the
     * generated query, e.g.: ... and (exp1 or exp2) and ..., assuming the old connective is "and", and the
     * new connective is "or".
     * @param newConnective New connective of the parameters.
     * @return Sub-parameters with the given connective.
     */
    public Parameters addSubParameters(String newConnective) {
        if (connective.Equals(newConnective)) {
            return this;
        } else {
            Parameters newParams = new Parameters(alias, newConnective, queryParamCounter);
            subParameters.Add(newParams);
            return newParams;
        }
    }

    /**
     * Adds negated parameters, by default with the "and" connective. These paremeters will be grouped in parentheses
     * in the generated query and negated, e.g. ... not (exp1 and exp2) ...
     * @return Negated sub paremters.
     */
    public Parameters addNegatedParameters() {
        Parameters newParams = new Parameters(alias, AND, queryParamCounter);
        negatedParameters.Add(newParams);
        return newParams;
    }

    public void addWhere(String left, String op, String right) {
        addWhere(left, true, op, right, true);
    }

    public void addWhere(String left, bool addAliasLeft, String op, String right, bool addAliasRight) {
        StringBuilder expression = new StringBuilder();

        if (addAliasLeft) { expression.Append(alias).Append("."); }
        expression.Append(left);

        expression.Append(" ").Append(op).Append(" ");

        if (addAliasRight) { expression.Append(alias).Append("."); }
        expression.Append(right);

        expressions.Add(expression.ToString());
    }

    public void addWhereWithParam(String left, String op, Object paramValue) {
        addWhereWithParam(left, true, op, paramValue);
    }

    public void addWhereWithParam(String left, bool addAlias, String op, Object paramValue) {
        String paramName = generateQueryParam();
        localQueryParamValues.Add(paramName, paramValue);

        addWhereWithNamedParam(left, addAlias, op, paramName);
    }

    public void addWhereWithNamedParam(String left, String op, String paramName) {
        addWhereWithNamedParam(left, true, op, paramName);
    }

    public void addWhereWithNamedParam(String left, bool addAlias, String op, String paramName) {
        StringBuilder expression = new StringBuilder();

        if (addAlias) { expression.Append(alias).Append("."); }
        expression.Append(left);
        expression.Append(" ").Append(op).Append(" ");
        expression.Append(":").Append(paramName);

        expressions.Add(expression.ToString());
    }

    public void addWhereWithParams(String left, String opStart, Object[] paramValues, String opEnd) {
        StringBuilder expression = new StringBuilder();

        expression.Append(alias).Append(".").Append(left).Append(" ").Append(opStart);

        for (int i=0; i<paramValues.Length; i++) {
            Object paramValue = paramValues[i];
            String paramName = generateQueryParam();
            localQueryParamValues.Add(paramName, paramValue);
            expression.Append(":").Append(paramName);

            if (i != paramValues.Length - 1)
            {
                expression.Append(", ");
            }
        }

        expression.Append(opEnd);

        expressions.Add(expression.ToString());
    }

    public void addWhere(String left, String op, IQueryBuilder right) {
        addWhere(left, true, op, right);
    }

    public void addWhere(String left, bool addAlias, String op, IQueryBuilder right) {
        StringBuilder expression = new StringBuilder();

        if (addAlias) {
            expression.Append(alias).Append(".");
        }

        expression.Append(left);

        expression.Append(" ").Append(op).Append(" ");

        expression.Append("(");
        right.Build(expression, localQueryParamValues);
        expression.Append(")");        

        expressions.Add(expression.ToString());
    }

    private void append(StringBuilder sb, String toAppend, MutableBoolean isFirst) {
        if (!isFirst.isSet()) {
            sb.Append(" ").Append(connective).Append(" ");
        }

        sb.Append(toAppend);

        isFirst.unset();
    }

    bool isEmpty() {
        return expressions.Count == 0 && subParameters.Count == 0 && negatedParameters.Count == 0;
    }

    void Build(StringBuilder sb, IDictionary<String, Object> queryParamValues) {
        MutableBoolean isFirst = new MutableBoolean(true);

        foreach (String expression in expressions) {
            append(sb, expression, isFirst);
        }

        foreach (Parameters sub in subParameters) {
            if (!(subParameters.Count == 0)) {
                append(sb, "(", isFirst);
                sub.Build(sb, queryParamValues);
                sb.Append(")");
            }
        }

        foreach (Parameters negated in negatedParameters) {
            if (!(negatedParameters.Count == 0)) {
                append(sb, "not (", isFirst);
                negated.Build(sb, queryParamValues);
                sb.Append(")");
            }
        }

        foreach (KeyValuePair<String, Object> pair in localQueryParamValues)
        {
            queryParamValues.Add(pair);
        }
    }
}

}