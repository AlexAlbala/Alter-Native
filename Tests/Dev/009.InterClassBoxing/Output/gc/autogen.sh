#! /bin/sh

set -e

# These version are ok, pre-1.7 is not.  Post 1.7 may produce a lot of
# warnings for unrelated projects, so prefer 1.7 for now.
am_version=
for v in 1.10 1.9 1.8 1.7; do
    if type -p &>/dev/null automake-$v; then
	am_version="-$v"
	break
    fi
done
if [ -z "$am_version" ]; then
    case "`automake --version`" in
	*\ 0.*|*\ 1.[0-6].*|*\ 1.[0-6]\ *)
	    echo "$0: Automake-1.7 or later is needed."
	    exit 2
	    ;;
    esac
fi

set -x
aclocal$am_version
autoconf
autoheader
automake$am_version -ac
libtoolize --automake --force
set +x
echo
echo "Ready to run './configure'."
echo
