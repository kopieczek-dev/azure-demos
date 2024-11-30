using GraphClientDemo;

Console.WriteLine("Hello, World!");

var demoGraphClient = new DemoGraphClient(
    clientId: "",
    tenantId: "",
    clientSecret: "");

var ret = await demoGraphClient.GetUserName("");

Console.Write("Username: ");
Console.WriteLine(ret);