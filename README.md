# SIT383-758-Class

# Project Repository Guide

Welcome to the **SIT383/758**.  
Please follow the instructions below to keep the project organised and avoid conflicts when collaborating.

---

# 📥 Cloning the Repository

1. Clone the repository to your local machine.
2. Open the project using **Unity / VS Code / Visual Studio**.

```bash
git clone <repository-url>
```

---

# 🔄 Pull & Push Instructions

Whenever you **pull or push changes**, please follow these steps carefully.

### 1️⃣ Pull the latest changes first

```bash
git pull origin main
```

### 2️⃣ Copy the `.gitignore` file

Whenever you pull or push, make sure you **copy and paste the `.gitignore` file into your Unity project folder**.

This helps prevent unnecessary files from being uploaded.

Files that should NOT be pushed include:

- Unity `Library` folder
- `Temp` files
- `Build` files
- `Logs`
- Cache files

### 3️⃣ Push your changes

After finishing your work:

```bash
git add .
git commit -m "Your message"
git push origin main
```

---

# 📁 Folder Structure

Each contributor must create **their own working folder**.

### Step 1 — Create your main folder

Create a folder with either:

- Your **name**
- A **cool project name**

Examples:

```
Sanjay's Do not Touch
XRExplorer
VRMaster
MetaBuilder
```

---

### Step 2 — Create weekly folders

Inside your folder, create **subfolders for each week**.

Example:

```
Sanjay's Do not touch
 ├── Week1
 ├── Week2
 ├── Week3
 ├── Week4
```

---

# 🗂 Example Week Folder Structure

Inside each week folder you can organise your work like this:

```
Week1
 ├── Scenes
 ├── Scripts
 ├── Materials
 ├── Prefabs
 ├── Assets
 └── Documentation
```

---

# 🎮 Unity / XR Development Guidelines

🕶 **Keep scenes lightweight**

Avoid uploading very large assets unless necessary.

📦 **Organise your assets properly**

Keep scripts, scenes and prefabs separated.

⚙ **Test your project before pushing**

Always ensure the Unity project runs without errors before committing changes.

---

# 🚫 Files That Should NOT Be Uploaded

The `.gitignore` file will automatically ignore:

```
Library/
Temp/
Build/
Logs/
.vs/
```

Please **do NOT upload these folders manually**.

They make the repository extremely large and cause conflicts.

---

# 🤝 Collaboration Rules

To avoid problems when multiple people are working:

✔ Always **pull before starting work**

✔ Keep commits **small and clear**

✔ Write **meaningful commit messages**

Example commit messages:

```
Added XR interaction script for Week2
Fixed teleportation issue
Updated VR menu UI
Improved scene lighting
```

---

# 💡 Pro Tips

🧠 If you are unsure about committing something, **ask before pushing**.

📁 Keep your weekly work organised so others can easily understand it.

🚀 Regular commits help avoid losing work.

---

# 🚀 Happy XR Building!

Let's create amazing **AR 🪄 | VR 🥽 | XR 🌐 experiences together!**