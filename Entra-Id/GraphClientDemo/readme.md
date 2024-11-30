This console application will query user's data from the Azure AD tenant using [Graph SDK](https://learn.microsoft.com/en-us/graph/sdks/sdks-overview)

1. Create a new Entra ID or Azure AD B2C tenant or use the existing one.
2. Register an Application
   1. Navigate to your tenant.
   2. In the left-hand menu, select **App registrations**.
   3. Click **New registration**.
   4. Enter a name for the application.
   5. Select **Supported account types**: Accounts in any identity provider or organizational directory (for authenticating users with user flows)
   6. Skip the **Redirect URI** because this is a console application.
   7. Click Register.
3. Go to the [`Program.cs`](/Entra-Id/GraphClientDemo/GraphClientDemo/Program.cs) and provide tenant details in `DemoGraphClient` constructor:
   1. `clientId`
   2. `tenantId`
   3. `clientSecret`
4. Go the the tenant in Azure Portal, select **Users** and choose any user. Find the **Object ID** and copy it.
5. Paste it as a `GetUserName()` method parameter (line 10). 
6. Run the program.