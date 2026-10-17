//SEC "compiler"
//basically just a port of an existing pico 8 thing i did
//
//pico 8 code*, for refrence
//program=split(program,"\n",false)
// labels={}
// for i=1,#program do
// 	pi=program[i]
// 	if cont(pi,";") then print("clipped comment at "..i-1) pi=rep(pi,split(pi,";")[2],"") pi=rep(pi,";","") end
// 	pi=rep(pi,"\t","")
// 	program[i]=pi
// 	if program[i][1]==":" then
// 		print("tagged line at "..i-1)
// 		labels[pi]=i-1
// 	end
// end
// for i=1,#program do
// 	pi=program[i]
// 	pi=rep(pi,"⬅️","\n")
// 	if pi[1]==">" then
// 		print("set jump tag at "..i-1)
// 		tag=sub(pi,2)
// 		pi=rep(pi,tag,labels[tag])
// 	end
// 	program[i]=pi
// end
// for i=1,#program do
// 	if cont(program[i],":") or program[i]=="" then program[i]="?" end
// end
// comp=program[1]
// for i=2,#program do
// 	comp..="|"..program[i]
// end
// --comp='"'..rep(upper(comp),"\n","\\n")..'",'
// comp=upper(comp)
// print("\ndone!\n\n"..comp)
// printh(comp,"@clip")

//using System.IO;
//"Using directive is unnecessary."

Console.WriteLine("type directory to input file");
string dir1=Console.ReadLine();
Console.WriteLine("type directory to output file");
string dir2=Console.ReadLine();
string inprogram=File.ReadAllText(dir1);

string[] outprogram=inprogram.Split('\n');
Dictionary<string,int> labels=[];
//oh thats helpful, VS can autocomplete whole syntax given the right situation
for(int i=0; i<outprogram.Length; i++) //pass 1
{
    string pi=outprogram[i];
    if(pi.Contains("//")) // this is less common than ; when printing text
    {
        Console.WriteLine("comment "+i);
        pi=pi.Split("//")[0];
    }
    pi=pi.Replace("\t","").TrimEnd('\r');
    outprogram[i]=pi;
    if(pi.StartsWith("`")) //SEC does not use '`' as a command
    {
        Console.WriteLine("label "+i);
        labels[pi[1..]]=i;
    }
}
for(int i=0; i<outprogram.Length; i++) //pass 2
{
    string pi=outprogram[i];
    pi=pi.Replace("\\n","\n").TrimEnd('\r');
    if(pi.StartsWith('>'))
    {
        Console.WriteLine("used label "+i);
        string tag=pi[1..]; //fix
        pi=pi.Replace("`"+tag,labels[tag].ToString());
    }
    outprogram[i]=pi;
}
for(int i=0; i<outprogram.Length; i++) //pass 3
{
    if(outprogram[i].StartsWith('`')||outprogram[i]=="") // first time i had to use "||" instead of "or"
    {
        outprogram[i]="?";
    }
}

File.WriteAllText(dir2,string.Join('|',outprogram));