﻿https://deanhume.com/a-simple-guide-to-using-oauth-with-c/
https://stackoverflow.com/questions/4002847/oauth-with-verification-in-net


1. Register your app with the service that you are developing it for. e.g. Twitter, Twitpic, SoundCloud etc. 
   You will receive a consumer key and secret.
2. You, the developer of the app then initiates the OAuth process by passing the consumer key and the consumer secret
3. The service will return a Request Token to you.
4. The user then needs to grant approval for the app to run requests.
5. Once the user has granted permission you need to exchange the request token for an access token.
6. Now that you have received an access token, you use this to sign all http requests with your credentials and access token.