
Pfrom = c(20,20);
Pto = c(100,100);

radius = 10;

ER <- function(Pfrom, Pto, radius) {
  x1 = Pfrom[1];
  y1 = Pfrom[2];
  x2 = Pto[1];
  y2 = Pto[2];
  
  
  
  if (x2 == x1) {
    theta = ifelse(y2 >= y1, pi/2, -pi/2);
  } else {
    theta = atan((y2-y1)/(x2-x1));
  }
  
  if (x2 < x1) {
    theta = theta + pi;
  }
  
  p1 = Pfrom + radius * c(cos(theta + pi/2), sin(theta+pi/2));
  p2 = Pfrom + radius * c(cos(theta + 1.5*pi), sin(theta+1.5*pi));
  p3 = Pto + radius * c(cos(theta - 0.5*pi), sin(theta- 0.5*pi));
  p4 = Pto + radius * c(cos(theta + 0.5*pi), sin(theta+ 0.5*pi));
  
  # if (sign((x2-x1) * (y2-y1)>=0)) {
  #   thetas1 = seq(from=theta + 0.5*pi, to= theta+1.5*pi, length.out=10);
  #   thetas2 = seq(from=theta - 0.5*pi, to= theta+0.5*pi, length.out=10);
  #   
  #   p12 =  cbind(Pfrom[1] + radius * cos(thetas1), Pfrom[2] + radius*sin(thetas1));
  #   p34 =  cbind(Pto[1] + radius * cos(thetas2), Pto[2] + radius*sin(thetas2));
  #   
  #   rr=rbind(p1,p12, p2, p3, p34, p4,p1);
  # } else {
    thetas1 = seq(from=theta + 0.5*pi, to= theta+1.5*pi, length.out=10);
    thetas2 = seq(from=theta - 0.5*pi, to= theta+0.5*pi, length.out=10);
    
    p12 =  cbind(Pfrom[1] + radius * cos(thetas1), Pfrom[2] + radius*sin(thetas1));
    p34 =  cbind(Pto[1] + radius * cos(thetas2), Pto[2] + radius*sin(thetas2));
    
    
    rr=rbind(p1,p12, p2, p3, p34, p4,p1);
    
  #  }
  
}
els = ER(c(20,20),c(100,100), 10);
plot(c(0,150),c(0,150),"n");
lines(els[,1],els[,2])

els2 = ER(c(100,20), c(20,100), 10)
lines(els2[,1],els2[,2],"l", col="red");

els3 = ER(c(20,60), c(100,60), 15);
lines(els3[,1],els3[,2],"l", col="blue");

els4 = ER(c(60,20), c(60,100), 10);
lines(els4[,1],els4[,2],"l", col="violet");

els4 = ER(c(70,100), c(70,20), 10);
lines(els4[,1],els4[,2],"l", col="brown");

plot(c(0,220),c(0,220),"n");
nrandom = 100;
x1=runif(100, min=10, max=200);
x2=runif(100, min=10, max=200);
y1=runif(100, min=10, max=200);
y2=runif(100, min=10, max=200);

rainb = rainbow(nrandom);
for (ii in 1:nrandom) {
  els = ER(c(x1[ii],y1[ii]),c(x2[ii],y2[ii]), 3);
  polygon(els[,1],els[,2], col=rainb[ii]);
}
