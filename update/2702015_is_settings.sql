-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.1.73-community - MySQL Community Server (GPL)
-- Server OS:                    Win64
-- HeidiSQL Version:             9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
-- Dumping data for table kdch_sk.is_settings: ~13 rows (approximately)
DELETE FROM `is_settings`;
/*!40000 ALTER TABLE `is_settings` DISABLE KEYS */;
INSERT INTO `is_settings` (`id`, `name`, `data`) VALUES
	(1, 'free_days', '1.1,6.1,6.4,18.4,21.4,1.5,8.5,5.7,29.8,1.9,15.9,1.11,17.11,24.12,25.12,26.12'),
	(2, 'opprogram', 'KdchLFUKDFNsPLimbova183340Bratislava'),
	(3, 'webstatus', 'true'),
	(4, 'kdch_shift_doctors', 'OUP,OddA,OddB,OP,Prijm'),
	(5, 'vykaz_doctors', 'prichod,obed.zac,obed.koniec,odchod,hodiny,noc.praca,mzd.zvyhod,sviatok,akt1,akt2,neakt1,neakt2,neakt3'),
	(6, 'typ_vykaz', 'normDen,malaSluzba,malaSluzba2,velkaSluzba,velkaSluzba2,velkaSluzba2a,sviatokVikend,sviatok,exday,sviatokNieVikend'),
	(7, 'hodiny_vykaz', '7,12:30,13:00,15,7.5,0,0,0,0,0,0,0,0\r\n7,12:30,13:00,19,11.5,5,0,0,5,0,7,0,0\r\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\r\n7,12:30,13:30,19,11.5,5,16.5,0,0,5,0,7,0,0\r\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\r\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\r\n7,12:30,13:00,19,11.5,5,16.5,16.5,0,5,0,7,0,0\r\n7,12:30,13:00,19,11.5,5,0,16.5,0,5,0,7,0,0\r\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\r\n0,0,0,0,7.5,0,0,0,0,0,0,0,0,0'),
	(11, 'bduchaj_vykaz', '7,12:30,13:00,15,7.5,0,0,0,0,0,0,0,0|7,12:30,13:00,19,11.5,5,0,0,5,0,7,0,0|0,0,0,0,0,0,0,0,0,0,0,0,0|7,12:30,13:30,19,11.5,5,16.5,0,0,5,0,7,0|0,0,0,0,0,0,0,0,0,0,0,0,0|0,0,0,0,0,0,0,0,0,0,0,0,0|7,12:30,13:00,19,11.5,5,16.5,16.5,0,5,0,7,0|7,12:30,13:00,19,11.5,5,0,16.5,0,5,0,7,0|0,0,0,0,0,0,0,0,0,0,0,0,0|0,0,0,0,7.5,0,0,0,0,0,0,0,0'),
	(12, 'KDCH_shifts_nurse', 'D1,D2,A1,RA,S1,S2,N1,N2,A2'),
	(13, 'KDCH_vykaz_nurse', 'prichod,obed.zac,obed.konc,odchod,zuct.hod,nadcas,nocna.praca,sobot.nedel,sviatok'),
	(14, 'KDCH_nurse_vykaz_riadky', 'dennka,nocna,nocna2'),
	(15, 'KDCH_nurse_vykaz_hod', 'D,6.00,12.30,13.00,18.00,11.5,0,0,0,11.5\r\nN,18.00,23.00,23.30,0,11.5,0,0,8\r\n0,0,0,0,0,6.00'),
	(16, '2dk_shift_doctors', 'Konz,Odd,OupA,OupB,Expe,KlAmb'),
	(17, 'mkabat_vykaz', '7,12:30,13:30,15,7.5,0,0,0,0,0,0,0,0|7,12:30,13:00,19,11.5,5,0,0,5,0,7,0,0|0,0,0,0,0,0,0,0,0,0,0,0,0|7,12:30,13:30,19,11.5,5,16.5,0,0,5,0,7,0|0,0,0,0,0,0,0,0,0,0,0,0,0|0,0,0,0,0,0,0,0,0,0,0,0,0|7,12:30,13:00,19,11.5,5,16.5,16.5,0,5,0,7,0|7,12:30,13:00,19,11.5,5,0,16.5,0,5,0,7,0|0,0,0,0,0,0,0,0,0,0,0,0,0|0,0,0,0,7.5,0,0,0,0,0,0,0,0');
/*!40000 ALTER TABLE `is_settings` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
