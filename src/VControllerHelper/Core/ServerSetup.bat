netsh http delete urlacl url = http://+:%1/
netsh http add urlacl url=http://+:%1/  user=Everyone

netsh advfirewall firewall Delete rule name="PrfLauncher����HTTP����" dir=in protocol=tcp
netsh advfirewall firewall Add rule name="PrfLauncher����HTTP����" dir=in protocol=tcp localport=%1 action=allow