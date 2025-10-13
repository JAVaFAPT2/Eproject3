#!/bin/bash

# Vehicle Showroom Management - Environment Setup Script

echo "🔧 Setting up environment files for Vehicle Showroom Management"
echo "=============================================================="

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

print_status() {
    echo -e "${GREEN}✓${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}⚠${NC} $1"
}

print_error() {
    echo -e "${RED}✗${NC} $1"
}

# Check if .env already exists
if [ -f ".env" ]; then
    print_warning ".env file already exists. Backing up to .env.backup"
    cp .env .env.backup
fi

# Copy template to .env
if [ -f "env.template" ]; then
    cp env.template .env
    print_status "Created .env file from template"
else
    print_error "env.template file not found!"
    exit 1
fi

# Copy production template
if [ -f "env.production.template" ]; then
    cp env.production.template .env.production
    print_status "Created .env.production file from template"
fi

# Update .gitignore to protect .env files
if [ -f ".gitignore" ]; then
    if ! grep -q ".env" .gitignore; then
        echo "" >> .gitignore
        echo "# Environment files" >> .gitignore
        echo ".env" >> .gitignore
        echo ".env.*" >> .gitignore
        echo "!.env.template" >> .gitignore
        echo "!.env.production.template" >> .gitignore
        print_status "Updated .gitignore to protect .env files"
    else
        print_status ".env files already protected in .gitignore"
    fi
else
    # Create .gitignore if it doesn't exist
    cat > .gitignore << 'EOF'
# Environment files
.env
.env.*
!.env.template
!.env.production.template

# Build outputs
bin/
obj/
out/

# User-specific files
*.user
*.suo
*.cache

# IDE files
.vs/
.vscode/
.idea/

# OS files
.DS_Store
Thumbs.db

# Logs
*.log
logs/

# Runtime files
*.pid
*.seed
*.pid.lock
EOF
    print_status "Created .gitignore file"
fi

echo ""
echo "🎉 Environment setup completed!"
echo ""
echo "Next steps:"
echo "1. Edit .env file with your actual values"
echo "2. For production, edit .env.production file"
echo "3. Never commit .env files to git (they're now protected)"
echo ""
echo "Important environment variables to update:"
echo "- ConnectionStrings__MongoDB: Your MongoDB Atlas connection string"
echo "- Jwt__Key: Generate a secure random key"
echo "- EmailSettings__SmtpUsername: Your email address"
echo "- EmailSettings__SmtpPassword: Your email app password"
echo "- CloudinarySettings__CloudName: Your Cloudinary cloud name"
echo "- CloudinarySettings__ApiKey: Your Cloudinary API key"
echo "- CloudinarySettings__ApiSecret: Your Cloudinary API secret"
echo ""
echo "📁 Files created:"
echo "- .env (for local development)"
echo "- .env.production (for production deployment)"
echo "- .gitignore (updated to protect .env files)"
