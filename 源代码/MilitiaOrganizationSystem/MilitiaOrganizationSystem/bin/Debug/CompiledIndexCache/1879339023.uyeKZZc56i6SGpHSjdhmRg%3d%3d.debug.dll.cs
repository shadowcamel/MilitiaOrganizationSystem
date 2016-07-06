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

public class Index_Militias_CredentialNumbers : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Militias_CredentialNumbers()
	{
		this.ViewText = @"from militia in docs.Militias
select new {
	CredentialNumber = militia.CredentialNumber,
	DbName = militia.Place
}";
		this.ForEntityNames.Add("Militias");
		this.AddMapDefinition(docs => 
			from militia in ((IEnumerable<dynamic>)docs)
			where string.Equals(militia["@metadata"]["Raven-Entity-Name"], "Militias", System.StringComparison.InvariantCultureIgnoreCase)
			select new {
				CredentialNumber = militia.CredentialNumber,
				DbName = militia.Place,
				__document_id = militia.__document_id
			});
		this.AddField("CredentialNumber");
		this.AddField("DbName");
		this.AddField("__document_id");
		this.AddQueryParameterForMap("CredentialNumber");
		this.AddQueryParameterForMap("Place");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("CredentialNumber");
		this.AddQueryParameterForReduce("Place");
		this.AddQueryParameterForReduce("__document_id");
	}
}
