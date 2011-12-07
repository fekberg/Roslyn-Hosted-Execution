using System.ServiceModel;

[ServiceContract(Namespace = "http://example.com/RoslynCodeExecution")]
interface ICommandService
{

    [OperationContract]
    string Execute(string code);

}