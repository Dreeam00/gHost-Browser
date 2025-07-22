# IFoxer

![IFoxer Icon](IFoxer-Photoroom.png)

**IFoxer** is an open source lightweight web browser developed based on C# and .NET 9, with an emphasis on customizability and ease of learning. It uses `WebView2` as a rendering engine that supports the latest web standards and provides a modern browsing experience. This project aims to be a learning material for Windows Forms application development and a starting point for developers who want to create their own browser.

---

## ✨ Main features

IFoxer covers the basic functions that a standard web browser should have, while leaving room for customization.

* **Tab browsing**:
* Intuitive tab addition, switching, and closing functions.
* Change the order of tabs by dragging and dropping.
* Close tabs by middle-clicking the mouse.

* **Bookmark management**:
* Easily register the currently displayed page as a favorite.
* Display a list of bookmarks and transition to pages from there.

* **Browser History and Download Management**:
* Track sites you've visited and revisit them any time.
* Manage file downloads from the web and their history.
* **Advanced Customization**:
* **Search Engine**: Switch to your favorite search engine at any time, including Google, Bing, and DuckDuckGo.
* **Theme Color**: Freely change the accent color of the application and enjoy your own design.

---

## 💻 Technology Stack

This project is built using a combination of the following technologies.

* **C# 12 & .NET 9**: The latest C# and .NET framework that handles the core logic of the application.
* **Windows Forms**: Builds the UI as a desktop application.
* **Microsoft Edge WebView2**: A rendering engine based on Microsoft Edge (Chromium). It draws HTML, CSS, and JavaScript quickly and accurately.
* **PowerShell**: A custom installer that automates the installation process.

---

## 🚀 Installation and Setup

There are two ways to use IFoxer.

### Method 1: Use the installer (recommended)

This is the easiest and recommended method.

1. Download the latest `installer.7z` from the release page and unzip it.

2. Right-click `install.ps1` in the unzipped folder and select **[Run with PowerShell]**.

* **Note**: If the script execution is blocked, run `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process` in PowerShell and try again.

3. Follow the wizard displayed on the screen to set the installation destination, create shortcuts, etc.

4. The installer automatically detects the required components (.NET 9 SDK, WebView2 Runtime) and prompts you to install them if they are not present.

### Method 2: Build from source (for developers)

You can also join the development by building directly from the source code.

1. **Prerequisites**: Install [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

2. Get the source code from this repository.

* **If you use Git**:

``bash

git clone https://github.com/Dreeam00/gHost-Browser.git

``

* **If you don't use Git**:

Click the `Code` button in the upper right corner of this repository page and select `Download ZIP` to download the source code. After downloading, unzip the ZIP file to a location of your choice.

3. In the project root directory, run the following command to build.

``bash

dotnet build

``

4. If the build is successful, `IFoxer.exe` will be generated in the `bin/Debug/net9.0-windows/` directory. Run it.

---

## 💡 Basic Usage

* **Navigation**: Enter the URL in the address bar at the top to browse the website. Back, forward, and reload buttons are also available.
* **Settings**: Open the settings screen from the menu button at the top right and change the search engine and theme color.
* **Add Bookmark**: Press the star button next to the address bar to add the current page to your bookmarks.

---

## 🤝 Contribute

Contribute to this project! Bug reports, feature suggestions, pull requests, anything is welcome.

1. If there is something you want to improve, start a discussion by opening an **Issue**.

2. Fork and create a branch (`git checkout -b feature/AmazingFeature`) and make your changes.

3. Commit your changes (`git commit -m 'Add some AmazingFeature'`) and push them to the branch (`git push origin feature/AmazingFeature`).
4. Create a **Pull Request**.
