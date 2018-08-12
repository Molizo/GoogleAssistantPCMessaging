# Google Assistant PC Messaging
This is the companion app for the IFTTT applet at https://ifttt.com/applets/fL8CTDpz-google-assistant-to-pc-messages

This app receives messages sent from Google Assistant using Adafruit IO and IFTTT

## Setup
#### Step 1: Adafruit IO setup

First,create a free Adafruit IO account at https://accounts.adafruit.com/users/sign_up .Be sure to write down your username,as it will be required later.
Then,after activating your account,head to your dashboard (https://io.adafruit.com) and on the left panel,select 'View AIO key' and write down the Active Key displayed in the popup.
Continue by closing the dialog and by selecting 'Feeds' from the left panel. Select from the 'Actions' dropdown 'Create a new feed' and give it a name. After it was created,select it from below and from the panel on the right side of your display select feed information and write down the Key.

#### Step 2: IFTTT setup

Go to https://ifttt.com/join and create an account.BE SURE TO USE THE SAME E-MAIL AS THE GOOGLE ACCOUNT USED BY GOOGLE ASSISTANT!
After that,go to https://ifttt.com/applets/fL8CTDpz-google-assistant-to-pc-messages and toggle on the applet.You will be prompted to select the feed,and select the one you made in step 1.

#### Step 3: App setup

After completing step 1 and 2,go to https://github.com/Molizo/GoogleAssistantPCMessaging/releases ,download the latest version and run the executable file.
Upon starting the app for the first time,you will be prompted to enter a friendly name for the PC,the Adafruit IO username,the AIO key,and the feed key.

Congratulations! You have completed the setup process!

## Usage

In order to send messages,open Google Assistant and say:
- Send to PC <<MESSAGE TO SEND>>
- PC send <<MESSAGE TO SEND>>
- Send <<MESSAGE TO SEND>> to PC
