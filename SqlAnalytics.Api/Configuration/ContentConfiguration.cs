using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlAnalytics.Api.Configuration
{

  public class ContentConfiguration
  {
    public ContentConfiguration()
    {
    }

    public List<Content> Content { get; set; }
  }

  public class Content
  {
    public Content()
    {

    }

    public string Name { get; set; }
    public string Quote { get; set; }
    public string Cite { get; set; }
    public string Href { get; set; }
  }

}


