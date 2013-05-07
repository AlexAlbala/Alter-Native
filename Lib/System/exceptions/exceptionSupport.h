#pragma once
#include <boost/scope_exit.hpp>
#define finally BOOST_SCOPE_EXIT
#define finally_end BOOST_SCOPE_EXIT_END