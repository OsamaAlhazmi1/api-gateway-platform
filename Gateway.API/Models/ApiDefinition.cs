namespace Gateway.API.Models;

public class ApiDefinition
{
    public int Id {get; set;}
    public string Name {get;set;}= ""; 
    public string RoutePrefix {get;set;} = ""; 
    public string DownstreamPath {get;set;}="";
    public string DestinationAddress {get;set;}= "";

}
