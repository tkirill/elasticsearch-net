﻿using Elasticsearch.Net;
using FluentAssertions;
using Nest.Tests.MockData;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nest.Tests.Integration.Reproduce
{
	[TestFixture]
	public class Reproduce643Tests : IntegrationTests
	{

		/// <summary>
		/// https://github.com/Mpdreamz/NEST/issues/643
		/// </summary>
		[Test]
		public void TermsAggregationOnLongFieldShouldHaveKeysOnBucket()
		{
			var searchResult = this._client.Search<ElasticsearchProject>(s => s
				.Size(0)
				.Aggregations(aggs=>aggs
					.Terms("numericTerms", t=>t
						.Field(p=>p.LongValue)
						.Size(10)
					)
				)
			);

			searchResult.IsValid.Should().BeTrue();
			var terms = searchResult.Aggs.Terms("numericTerms");
			terms.Items.Should().NotBeEmpty().And.NotContain(p => p.Key.IsNullOrEmpty());

		}

	}
}
