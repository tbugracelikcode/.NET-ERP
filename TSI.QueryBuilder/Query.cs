﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;

namespace TSI.QueryBuilder
{
    public partial class Query : QueryFactory
    {
        public string Sql { get; set; }

        public string TableName { get; set; }

        public string Columns { get; set; }

        public object SqlResult { get; set; }

        public string JsonData { get; set; }

        public string WhereSentence { get; set; }

        public string JoinSeperator { get; set; }

        public string TablesJoinKeywords { get; set; }

        public bool UseIsDeleteInQuery { get; set; } = true;

        public bool IsMapQuery { get; set; } = true;

    }
}
