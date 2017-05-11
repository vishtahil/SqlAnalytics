﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyticsManager.Models
{
    public class SqlOptimizationHint
    {
        public string MatchedValue { get; set; }
        public string MatchedExpression { get; set; }
        public string MatchedSqlClause { get; set; }

        public string MatchedSqlText { get; set; }
    }
    public enum SqlClause
    {
        INNER_JOIN,
        LEFT_JOIN,
        RIGHT_JOIN,
        FULL_JOIN,
        JOIN,
        CROSS_JOIN,
        NESTED_LESS_THAN,
        NESTED_GREATER_THAN,
        NESTED_EQUAL_TO,
        NESTED_IN,
        NESTED_EXISTS,
        NESTED_SELECT,
        NESTED_JOIN,
        FULL_OUTER_JOIN,
        RIGHT_OUTER_JOIN,
        LEFT_OUTER_JOIN,
        NESTED_SELECT_FROM,
        BLOCK_SELECT,
        ORDER_BY,
        LIKE_BEGIN,
        WHERE_FUNCTION_PRECEEDING,
        WHERE_NOT_EQUAL_TO
    }
}
