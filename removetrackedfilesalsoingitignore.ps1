# Get files matching exclusionsfrom .gitignore
# Excluding comments and empty lines
$ignoreFiles =  gc .gitignore | ?{$_ -notmatch  "#"} |  ?{$_ -match  "\S"} | % {
                    $ignore = "*" + $_ + "*"
                    (gci -r -i $ignore).FullName
                }
$ignoreFiles = $ignoreFiles| ?{$_ -match  "\S"}

# Remove each of these file from Git 
$ignoreFiles | % { git rm --cached $_}

git add .