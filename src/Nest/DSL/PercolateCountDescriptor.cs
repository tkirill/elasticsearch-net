﻿using Elasticsearch.Net;
using Nest.Resolvers;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nest
{
	[DescriptorFor("CountPercolate")]
	public partial class PercolateCountDescriptor<T,K> : IndexTypePathTypedDescriptor<PercolateCountDescriptor<T, K>, PercolateCountRequestParameters, T> 
		, IPathInfo<PercolateCountRequestParameters> 
		where T : class
		where K : class
	{
		[JsonProperty(PropertyName = "query")]
		internal IQueryContainer _Query { get; set; }

		[JsonProperty(PropertyName = "doc")]
		internal K _Document { get; set; }

		/// <summary>
		/// The object to perculate
		/// </summary>
		public PercolateCountDescriptor<T, K> Object(K @object)
		{
			this._Document = @object;
			return this;
		}

		/// <summary>
		/// Optionally specify more search options such as facets, from/to etcetera.
		/// </summary>
		public PercolateCountDescriptor<T, K> Query(Func<QueryDescriptor<T>, QueryContainer> querySelector)
		{
			querySelector.ThrowIfNull("querySelector");
			var d = querySelector(new QueryDescriptor<T>());
			this._Query = d;
			return this;
		}

		ElasticsearchPathInfo<PercolateCountRequestParameters> IPathInfo<PercolateCountRequestParameters>.ToPathInfo(IConnectionSettingsValues settings)
		{
			var pathInfo = base.ToPathInfo(settings, this._QueryString);
			//.NET does not like sending data using get so we use POST
			pathInfo.HttpMethod = PathInfoHttpMethod.POST;
			return pathInfo;
		}
	}
}
