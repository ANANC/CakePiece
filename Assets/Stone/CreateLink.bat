 @echo off

@rem 【自行修改】项目目录地址
set GamePath=F:\SelfEnvironment\CakePiece
@rem 【自行修改】Stone框架的目录地址
set StonePath=F:\SelfEnvironment\StoneFramework

@rem 项目框架文件夹名
set MyGameStoneName=StoneLink
@rem 项目框架引用地址
set MyGameStonePath=%GamePath%\Assets\Stone\%MyGameStoneName%

echo 确定需要创建软连接吗？
echo Stone框架地址： %StonePath%
echo Game框架地址：%MyGameStonePath%
pause

if not exist "%StonePath%" (
	echo 连接失败，不存在Stone框架的目录地址
	exist
)

if exist "%MyGameStonePath%" (
	rmdir "%MyGameStonePath%"
)

mklink /D "%MyGameStonePath%" "%StonePath%"

echo 连接完成
pause