//Basic Particle Animation
//Author: Brandon John-Freso (Particle Animation)
//Author: Gerard Solé (Change particles to Bokeh rounds)

window.bokeh = function bokeh(){
	var W, H,
	canvas, ctx, 
	particleCount = 350,
	particles = [];

	W = window.innerWidth;
	H = window.innerHeight;
	canvas = $("#canvas").get(0);
	canvas.width = W;
	canvas.height = H;
	ctx = canvas.getContext("2d");
	/*ctx.clearRect(0,0,canvas.width,canvas.height);*/
	rand = function(rMi, rMa){return ~~((Math.random()*(rMa-rMi+1))+rMi);}

//Setup particle class
function Particle(){
	this.radius = rand(5,25);
	this.x = rand(0,W);
	this.y = rand(rand(H-H/2,H-H/5),rand(H,H/3));
	this.alpha = rand(1,400)/500;
	this.hue = rand(12, 216);
	this.lightness = rand(10, rand(70,100));
	this.saturation = rand(100, 900);
	this.lineWidth = this.radius/40;
	this.shadowBlur = rand(this.radius/90, this.radius/4);

	this.fillColor = 'hsla('+this.hue+','+this.saturation+'%,'+this.lightness+'%,'+this.alpha+')',
	this.strokeColor = 'hsla('+this.hue+','+this.saturation+'%,'+this.lightness+'%,'+this.alpha+')',
	this.shadowColor = 'hsla('+this.hue+','+this.saturation*2+'%,'+this.lightness*2+'%,'+this.alpha*2.5+')';

	this.direction ={"x": -1 + Math.random()*2, "y": -1 + Math.random()*2};
	this.vx = 0.5 * Math.random();
	this.vy = 0.5 * Math.random();
	this.move = function(){
		this.x += this.vx * this.direction.x;
		this.y += this.vy * this.direction.y;
	};
	this.changeDirection = function(axis){
		this.direction[axis] *= -1;
	};
	this.draw = function() {
		ctx.beginPath();
		ctx.fillStyle = this.fillColor;
		ctx.shadowColor = this.shadowColor;
		ctx.shadowBlur = this.shadowBlur;
		ctx.strokeStyle = this.strokeColor;	
		ctx.lineWidth = this.lineWidth;
		ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2, false);
		ctx.fill();
	};
	this.boundaryCheck = function(){
		if(this.x >= W){
			this.x = W;
			this.changeDirection("x");
		}
		else if(this.x <= 0){
			this.x = 0;
			this.changeDirection("x");
		}
		if(this.y >= H){
			this.y = H;
			this.changeDirection("y");
		}
		else if(this.y <= 0){
			this.y = 0;
			this.changeDirection("y");
		}
	};
} //end particle class

function clearCanvas(){
	ctx.clearRect(0,0, W, H);
} //end clear canvas

function createParticles(){
	for (var i = particleCount-1; i >= 0; i--) {
		p = new Particle();
		particles.push(p);
	}
}// end createParticles

function drawParticles(){
	for (var i = particleCount-1; i >= 0; i--){
		p = particles[i];
		p.draw();
	}
} //end drawParticles

function updateParticles(){
	for(var i = particles.length - 1; i >=0; i--){
		p = particles[i];
		p.move();
		p.boundaryCheck();

	}
}//end updateParticles

function initParticleSystem(){
	createParticles();
	drawParticles();
}

function animateParticles(){
	clearCanvas();
	drawParticles();
	updateParticles();
}

initParticleSystem();
setInterval(animateParticles,1000/25);


}
$(window).bind('resizeEnd', function() {
	if(isDynamic){
		$("#canvas").remove();
		$('body').prepend( "<canvas id=\"canvas\"></canvas>" );
		bokeh();
	}
});
$(window).resize(function() {
	if(this.resizeTO) clearTimeout(this.resizeTO);
	this.resizeTO = setTimeout(function() {
		$(this).trigger('resizeEnd');
	}, 5);
});
$(function(){
	bokeh();
});