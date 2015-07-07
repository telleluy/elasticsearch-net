﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nest.QueryDsl.Visitor
{
	public class DslPrettyPrintVisitor : IQueryVisitor
	{
		private readonly StringBuilder _sb;
		private string _final;
		private ElasticInferrer _infer;

		public string PrettyPrint
		{
			get
			{
				if (_final.IsNullOrEmpty())
					_final = _sb.ToString();
				return _final;
			}
		}

		public DslPrettyPrintVisitor(IConnectionSettingsValues settings)
		{
			this._sb = new StringBuilder();
			this._infer = settings.Inferrer;
		}

		public virtual int Depth { get; set; }
		public virtual VisitorScope Scope { get; set; }

		protected bool IsVerbatim { get; set; }
		protected bool IsConditionless { get; set; }
		protected bool IsStrict { get; set; }
		
		public virtual void Visit(IQueryContainer baseQuery)
		{
			this.IsConditionless = baseQuery.IsConditionless;
			this.IsStrict = baseQuery.IsStrict;
			this.IsVerbatim = baseQuery.IsVerbatim;
		}

		public virtual void Visit(IQuery query)
		{
			//Write("");
		}

		private void Write(string queryType, Dictionary<string, string> properties)
		{
			properties = properties ?? new Dictionary<string, string>();
			var props = string.Join(", ", properties.Select(kv => "{0}: {1}".F(kv.Key, kv.Value)));
			var indent = new String('-',(Depth -1) * 2);
			var scope = this.Scope.GetStringValue().ToLowerInvariant();
			_sb.AppendFormat("{0}{1}: {2} ({3}){4}", indent, scope, queryType, props, Environment.NewLine);
		}
		private void Write(string queryType, PropertyPath fieldName = null)
		{
			this.Write(queryType, fieldName == null 
				? null 
				: new Dictionary<string, string> {{"field", this._infer.PropertyPath(fieldName)}});
		}

		public virtual void Visit(IBoolQuery query)
		{
			Write("bool");
		}

		public virtual void Visit(IBoostingQuery query)
		{
			Write("boosting");
		}

		public virtual void Visit(ICommonTermsQuery query)
		{
			Write("common_terms", query.Field);
		}

		public virtual void Visit(IConstantScoreQuery query)
		{
			Write("constant_score");
		}

		public virtual void Visit(IDisMaxQuery query)
		{
			Write("dis_max");
		}

		public virtual void Visit(IFilteredQuery query)
		{
			Write("filtered");
		}

		public virtual void Visit(IFunctionScoreQuery query)
		{
			Write("function_score");
		}

		public virtual void Visit(IFuzzyQuery query)
		{
			Write("fuzzy", query.Field);
		}

		public virtual void Visit(IGeoShapeQuery query)
		{
			Write("geo_shape", query.Field);
		}

		public virtual void Visit(IHasChildQuery query)
		{
			Write("has_child");
		}

		public virtual void Visit(IHasParentQuery query)
		{
			Write("has_parent");
		}

		public virtual void Visit(IIdsQuery query)
		{
			Write("ids");
		}

		public virtual void Visit(IIndicesQuery query)
		{
			Write("indices");
		}

		public virtual void Visit(IMatchQuery query)
		{
			Write("match", query.Field);
		}

		public virtual void Visit(IMatchAllQuery query)
		{
			Write("match_all");
		}

		public virtual void Visit(IMoreLikeThisQuery query)
		{
			Write("more_like_this");
		}

		public virtual void Visit(IMultiMatchQuery query)
		{
			Write("multi_match");
		}

		public virtual void Visit(INestedQuery query)
		{
			Write("nested");
		}

		public virtual void Visit(IPrefixQuery query)
		{
			Write("prefix");
		}

		public virtual void Visit(IQueryStringQuery query)
		{
			Write("query_string");
		}

		public virtual void Visit(IRangeQuery query)
		{
			Write("range");
		}

		public virtual void Visit(IRegexpQuery query)
		{
			Write("regexp");
		}

		public virtual void Visit(ISimpleQueryStringQuery query)
		{
			Write("simple_query_string");
		}

		public virtual void Visit(ISpanFirstQuery query)
		{
			Write("span_first");
		}

		public virtual void Visit(ISpanNearQuery query)
		{
			Write("span_near");
		}

		public virtual void Visit(ISpanNotQuery query)
		{
			Write("span_not");
		}

		public virtual void Visit(ISpanOrQuery query)
		{
			Write("span_or");
		}

		public virtual void Visit(ISpanTermQuery query)
		{
			Write("span_term");
		}

		public virtual void Visit(ITermQuery query)
		{
			Write("term", query.Field);
		}

		public virtual void Visit(IWildcardQuery query)
		{
			Write("wildcard");
		}

		public virtual void Visit(ITermsQuery query)
		{
			Write("terms");
		}

		public virtual void Visit(ITypeQuery filter)
		{
			Write("type");
		}

		public virtual void Visit(IMissingQuery filter)
		{
			Write("missing");
		}

		public virtual void Visit(IGeoPolygonQuery filter)
		{
			Write("geo_polygon");
		}

		public virtual void Visit(IGeoDistanceRangeQuery filter)
		{
			Write("geo_distance_range");
		}

		public virtual void Visit(IGeoDistanceQuery filter)
		{
			Write("geo_distance");
		}

        public virtual void Visit(IGeoHashCellQuery filter)
        {
            Write("geohash_cell");
        }

		public virtual void Visit(IGeoBoundingBoxQuery filter)
		{
			Write("geo_bounding_box");
		}

		public virtual void Visit(IExistsQuery filter)
		{
			Write("exists");
		}

		public void Visit(IScriptQuery filter)
		{
			Write("script");
		}
	}
}
