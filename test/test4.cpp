#include <iostream>
#include <string>
using namespace std;

int main(int argc, char*argv[])
{
	int x, y;
	string s;
	for (int i=0; i<argc; i++)
		cout << argv[i] << endl;
	cin >> x >> y >> s;
	cout << x << y << s;
}