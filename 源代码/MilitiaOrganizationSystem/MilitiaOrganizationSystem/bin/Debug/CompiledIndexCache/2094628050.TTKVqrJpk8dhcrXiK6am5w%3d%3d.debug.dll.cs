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

public class Index_Militias_All : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Militias_All()
	{
		this.ViewText = @"from militia in docs.Militias
select new {
	Group = militia.Group,
	Place = militia.Place,
	Education = militia.Education
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from militia in ((IEnumerable<dynamic>)docs)
			where string.Equals(militia["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				Group = militia.Group,
				Place = militia.Place,
				Education = militia.Education,
				__document_id = militia.__document_id
			});
		this.AddField("Group");
		this.AddField("Place");
		this.AddField("Education");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("Group");
		this.AddQueryParameterForMap("Place");
		this.AddQueryParameterForMap("Education");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("Group");
		this.AddQueryParameterForReduce("Place");
		this.AddQueryParameterForReduce("Education");
		this.AddQueryParameterForReduce("__document_id");
	}
}
