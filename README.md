## Two software
# CryptoSoft
Our application to help you to encrypt and decrypt different type files.

#Important
For run you need the following NuGet added to EasySave :
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Binder
- Microsoft.Extensions.Configuration.Json
- Newtonsoft.Json
- System.Windows.Extensions
- System.Threading
### Care some package aren't avaible on Visual, you need to install them on Rider

# EasySave - Save Software for 2F Roaming

## The link to the executable is the following :
EasySave/bin/Debug/net7.0/

## For project management (where are located our US) we use Zenhub
Zenhub is an extension for your browser
chrome link : https://chrome.google.com/webstore/detail/zenhub-for-github/ogcgkffhplmphkaahpmffcafajaocjbd

# For Developers
## Important if you work on Visual Studio
First you will need the 2022 version for make working the application. Cause it disposes of NET Core 7.0 that we currently use in our application.

## A- Starting with GIT
1. Open GIT Bash
2. Do git clone "https://github.com/[your_username_github]/EasySave.git" (you can find this url if you click on clone on github, a path should be shown)

## B- Starting programming
   1. On git bash, go on the project that you previously clone (name : EasySave)
   2. Recuperate the common repository on Github with :    git pull (very important for have the last version of the application before programming)
   3. Create a branch with the name 'EasySave-[number_of_US]' with :    git branch [name]
   4. Change of branch with :    git checkout [name]

## C- Use git on Visual Studio 2022 for share changes
   1. Open the folder of the project previously clone (name: EasySave)
   2. For push on Visual Studio you need to configure username and email (on the right panel) : use the same as github
   You can now use all command git on Visual Studio.
   
## If you encounter some problems with the visibility of the branch
git checkout -b [branch_name] origin/[branch_name]

## More explanations
At the right panel :
    - The modification correspond at what you have modified, when you commit it will disappear (same behaviour as red file when you do "git status" on bash)
    Notice that you can see specific changes on a file with a solid line next to the number of the line.
    - index changes: it corresponds to what you can commit and push on the github (same behaviour as "git add [file_name]" on git bash).
    - All the changes you chose are indexed, you can now commit for share them with the team, on Visual it correspond to the textfield with validate index changes, write a short message corresponding to what you've done and click on this button (same behaviour as "git push" on bash).
