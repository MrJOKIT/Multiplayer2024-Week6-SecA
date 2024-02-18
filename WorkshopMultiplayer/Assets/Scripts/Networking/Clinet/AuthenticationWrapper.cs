using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = global::AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxRetries = 5)
    {
        if (AuthState == AuthState.Authenticate)
        {
            return AuthState;
        }

        if (AuthState == global::AuthState.Authenticating)
        {
            Debug.LogWarning("Already authicating!");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymuslyAsync(maxRetries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == global::AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    private static async Task SignInAnonymuslyAsync(int maxRetire)
    {
        AuthState = AuthState.Authenticating;

        int retries = 0;
        while (AuthState == AuthState.Authenticating && retries < maxRetire)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticate;
                    break;
                }
            }
            catch (AuthenticationException e)
            {
                Debug.LogError(e);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException exeption)
            {
                Debug.LogError(exeption);
                AuthState = AuthState.Error;
            }
            
            retries++;
            await Task.Delay(1000);
        }
        
        if (AuthState != AuthState.Authenticate)
        { 
            Debug.LogWarning($"Player was not signed in successfully after {retries} retries");
            AuthState = global::AuthState.TimeOut;
        }
    }

    
}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticate,
    Error,
    TimeOut,
}
