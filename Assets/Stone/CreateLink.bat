 @echo off

@rem �������޸ġ���ĿĿ¼��ַ
set GamePath=F:\SelfEnvironment\CakePiece
@rem �������޸ġ�Stone��ܵ�Ŀ¼��ַ
set StonePath=F:\SelfEnvironment\StoneFramework

@rem ��Ŀ����ļ�����
set MyGameStoneName=StoneLink
@rem ��Ŀ������õ�ַ
set MyGameStonePath=%GamePath%\Assets\Stone\%MyGameStoneName%

echo ȷ����Ҫ������������
echo Stone��ܵ�ַ�� %StonePath%
echo Game��ܵ�ַ��%MyGameStonePath%
pause

if not exist "%StonePath%" (
	echo ����ʧ�ܣ�������Stone��ܵ�Ŀ¼��ַ
	exist
)

if exist "%MyGameStonePath%" (
	rmdir "%MyGameStonePath%"
)

mklink /D "%MyGameStonePath%" "%StonePath%"

echo �������
pause