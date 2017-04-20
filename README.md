# AutomaticSpeachRecognition
practical exercise for modul ASP
This Project is part of the modul ASP of the FH-Muenster.
It should calculate the probability of the next letter of a input based on the previous letters. To make this exercise less complex it is just use an T9-system.

1:={}
2:={a,b,c}
3:={d,e,f}
4:={g,h,i}
5:={j,k,l}
6:={m,n,o}
7:={p,q,r,s}
8:={t,u,v}
9:={w,x,y,z}

To calculate the probability this project will use a weighted graphs (tree) as an Markov-chains n order.
The graphes will create by reading an ascii-text-file char by char and count the frequency of the letters.
The depth of the tree will by set by user. Separators like " ", ".", ",", linefeed, etc. where used as normal chars in tree and do not cut the depth.
The graph can be stored or loaded to or from file-system.

At the beginning just one graph will be used. in the future it should be possible to use more graphes with different depth at the same time to increase the accuracy.
