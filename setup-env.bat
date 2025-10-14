@echo off
echo  Setting up environment files for Vehicle Showroom Management
echo ==============================================================

REM Check if .env already exists
if exist ".env" (
    echo  .env file already exists. Backing up to .env.backup
    copy .env .env.backup
)

REM Copy template to .env
if exist "env.template" (
    copy env.template .env
    echo  Created .env file from template
) else (
    echo  env.template file not found!
    pause
    exit /b 1
)

REM Copy production template
if exist "env.production.template" (
    copy env.production.template .env.production
    echo  Created .env.production file from template
)

REM Update .gitignore to protect .env files
if exist ".gitignore" (
    findstr /C:".env" .gitignore >nul
    if errorlevel 1 (
        echo. >> .gitignore
        echo # Environment files >> .gitignore
        echo .env >> .gitignore
        echo .env.* >> .gitignore
        echo !.env.template >> .gitignore
        echo !.env.production.template >> .gitignore
        echo  Updated .gitignore to protect .env files
    ) else (
        echo  .env files already protected in .gitignore
    )
) else (
    REM Create .gitignore if it doesn't exist
    (
        echo # Environment files
        echo .env
        echo .env.*
        echo !.env.template
        echo !.env.production.template
        echo.
        echo # Build outputs
        echo bin/
        echo obj/
        echo out/
        echo.
        echo # User-specific files
        echo *.user
        echo *.suo
        echo *.cache
        echo.
        echo # IDE files
        echo .vs/
        echo .vscode/
        echo .idea/
        echo.
        echo # OS files
        echo .DS_Store
        echo Thumbs.db
        echo.
        echo # Logs
        echo *.log
        echo logs/
        echo.
        echo # Runtime files
        echo *.pid
        echo *.seed
        echo *.pid.lock
    ) > .gitignore
    echo  Created .gitignore file
)

echo.
echo  Environment setup completed!
echo.
echo Next steps:
echo 1. Edit .env file with your actual values
echo 2. For production, edit .env.production file
echo 3. Never commit .env files to git (they're now protected)
echo.
echo Important environment variables to update:
echo - ConnectionStrings__MongoDB: Your MongoDB Atlas connection string
echo - Jwt__Key: Generate a secure random key
echo - EmailSettings__SmtpUsername: Your email address
echo - EmailSettings__SmtpPassword: Your email app password
echo - CloudinarySettings__CloudName: Your Cloudinary cloud name
echo - CloudinarySettings__ApiKey: Your Cloudinary API key
echo - CloudinarySettings__ApiSecret: Your Cloudinary API secret
echo.
echo  Files created:
echo - .env (for local development)
echo - .env.production (for production deployment)
echo - .gitignore (updated to protect .env files)
echo.
pause
