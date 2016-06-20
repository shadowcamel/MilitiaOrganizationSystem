using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;

public class Index_Militias_GroupingByGroup : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Militias_GroupingByGroup()
	{
		this.ViewText = @"from militia in docs.Militias
select new {
	Group = militia.Group,
	Count = 1
}
from result in results
group result by result.Group into g
select new {
	Group = g.Key,
	Count = Enumerable.Sum(g, x => ((int)x.Count))
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from militia in ((IEnumerable<dynamic>)docs)
			where string.Equals(militia["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Group = militia.Group,
				Count = 1,
				__document_id = militia.__document_id
			});
		this.ReduceDefinition = results => 
			from result in results
			group result by result.Group into g
			select new {
				Group = g.Key,
				Count = Enumerable.Sum(g, (Func<dynamic, int>)(x => ((int)x.Count)))
			};
		this.GroupByExtraction = result => result.Group;
		this.AddField("Group");
		this.AddField("Count");
		this.AddQueryParameterForMap("Group");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Group");
		this.AddQueryParameterForReduce("__document_id");
	}
}
