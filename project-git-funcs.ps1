$branchs = git branch

$a_branch

foreach ($branch in $branchs) {
	if ($branch.Contains("*")) {
		$a_branch = $branch.Trim("*").Trim()
	}
}

. git-funcs

Switch -Wildcard ($a_branch) {
	"*.dev" {
		merge "master.fix" $a_branch
		merge "master" "master.fix"
		git checkout $a_branch
	}
	"*.fix" {
		merge "master.dev" $a_branch
		merge "master" "master.dev"
		git checkout $a_branch
	}
	default {
		merge "master.dev" $a_branch
		merge "master.fix" "master.dev"
		git checkout $a_branch
	}
}

$branchs = git remote

foreach ($branch in $branchs) {
	push-all $branch $True
}