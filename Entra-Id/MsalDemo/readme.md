How to create an Azure AD B2C instance and setup web application to use MSAL to authenticate users.

## Steps to Create an Azure B2C Instance

1. **Create an Azure AD B2C Tenant:**
   - Sign in to the [Azure portal](https://portal.azure.com/).
   - In the left-hand navigation pane, select **Create a resource**.
   - Search for **Azure Active Directory B2C** and select it.
   - Click **Create**.
   - Fill in the required information and click **Create**.

2. **Register an Application:**
   - Navigate to your Azure AD B2C tenant.
   - In the left-hand menu, select **App registrations**.
   - Click **New registration**.
   - Enter a name for the application.
   - Select **Supported account types**: Accounts in any identity provider or organizational directory (for authenticating users with user flows)
   - Set the **Redirect URI** to `https://localhost:5001/signin-oidc` (or your application's URL).
   - Make sure the correct settings are provided as the AD B2C use cache for the configuration. It may take some time to apply changes.
   - Click **Register**.


3. **Configure the Application:**
   - After registration, go to the **Authentication** section in Azure AD B2C tenant.
   - Under **Implicit grant and hybrid flows**, check the boxes for **ID tokens** and **Access tokens**.
   - Click **Save**.

4. **Create User Flows:**
   - In the left-hand menu, select **User flows**.
   - Click **New user flow**.
   - Select the **Sign up and sign in** policy.
   - Enter "b2c_1_susi" as the name for the policy and configure the required attributes and claims.
   - Click **Create**.
   - Optionally, repeat the steps to create **Password reset** and **Profile editing** policies.


5. Update your [`appsettings.json`](/MsalDemo/MsalDemo/appsettings.json) file with the tenant configuration:
    - `"Instance"`: "https://your-tenant-name.b2clogin.com"
    - `"Domain"`:  "your-tenant-name.onmicrosoft.com"
    - `"ClientId"`: "your-client-id",
    - `"SignUpSignInPolicyId"`: "b2c_1_susi",
    - `"CallbackPath"`: "/signin-oidc"
    - `"SignedOutCallbackPath"`: "/signout/B2C_1_susi"

6. Add a user to the tenant. Hit **New user** and **New external user**.

7. Run the application. Click "Sign In" link in the upper right corner and log in with the newly created user.