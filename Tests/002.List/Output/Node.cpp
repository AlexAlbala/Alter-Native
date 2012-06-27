#include "Node.h"
namespace List{

	Node::Node(){
		float f = Utils::random->NextDouble();
		this->value = (int)(f * 1000.0);
	}

}