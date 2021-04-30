using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveData : MonoBehaviour
{
    public InputField nameField;
    public InputField emailField;

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                UnityEngine.Debug.Log("Firebase works");
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                //OnFirebaseInitialized.Invoke();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
            //OnFirebaseInitialized.Invoke();
        });
    }

    public void PassUser()
    {
        WriteNewUser("1", nameField.text, emailField.text);
    }

    public void WriteNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void UpdateUser(string userId, string name, string email)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("users").Child(userId).SetValueAsync(name);
    }

    public void DeleteUser(string userId)
    {

    }
}

public class User
{
    public string username;
    public string email;

    public User() { }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}
