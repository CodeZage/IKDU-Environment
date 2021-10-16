# IKDU Environment 🤖

This is a Unity Project which is intended to act as a quickstart option for quickly setting up an environment. The goal is to make it easier for new students to get started and have some tools so they can start developing.

## Installing Git

Git might already be installed on your system. If you want to check you can open a new terminal or command prompt session and type git. If git is installed, you should see this: 

![image](https://user-images.githubusercontent.com/55112918/137598756-77aade78-d3dc-4394-863e-44ad3928e200.png)

If git is not installed, please go to the following website, and follow the installation instructions for your system's OS:

[Git](https://git-scm.com/)

Once installed you can now use git to manage your files.

## Accessing GitHub

To download the project onto your computer you're going to need to get access to GitHub. To use clone from GitHub you won't need an account but it is a good idea to make one. To do that first go to the link below:

[GitHub: Where the world builds software](https://github.com/)

Here you can sign up for an account. Once signed up you can now use GitHub.

## Cloning Project

To clone the project, open a terminal session and navigate to the folder you would like to hos your project. You might also be able to go to the folder using the file explorer on your system, right clicking, and pressing the open in terminal or similar command. Once there enter the following command to clone the project:

```
git clone https://github.com/CodeZage/IKDU-Environment.git
```

The project should clone itself into the folder you are currently in. Once cloned you can now open the project in a text editor, IDE or add it to Unity Hub depending on the project type. 

## Committing Changes

Once you have created some changes you might want to commit them to the GitHub repository. You cannot directly commit to the main branch however, instead you need to create what is known as a pull request. 

To do that first you need to create a new branch to host you changes. This can be done like so:

```csharp
git checkout -b my-branch // Replace my-branch with a descriptive name for your branch
```

Now that we have a branch, were going to create our changes and commit them. First, we need to add the changed files. The easiest is to add all the files at once, but if you want you can exclude files you aren't ready to commit yet. To add all the files, use on of the following commands. 

```csharp
git add -A // Adds all changes and new files in the directory
git add .  // Adds all changes and new files in the directory but excludes deletions
git add -u // Adds all modifications and deletions but not new files
```

Once the changes have been added they need to be committed to the local repository to create a commit which. This is a snapshot of the project and can be reverted to later should it be necessary. To commit the files, use the following command.  

```csharp
git commit -m "My commit message here"
```

You can also leave out the m and message to let git open a text editor where you can edit the message instead. 

Once committed you now need to push it to the remote branch located on the GitHub servers. To do this simply type the following: 

```jsx
git push --set-upstream origin my-branch // This if you do not have a branch on github
git push // This if the upstream branch has already been set
```

Now the changes have been pushed to GitHub and are ready to be merged via a pull request.

## Creating a Pull Request

Once you have committed the changes you need you might want to add them to the main branch. To do this you must create a pull request and get another team member to review and approve it. 

Go to the pull requests page and click on new pull request. Select the branches you would like to merge and create the pull request. Another person can then open and review it. Once successfully reviewed it can be merged and closed.
