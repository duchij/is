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
-- Dumping data for table kdch_sk.is_labels: ~5 rows (approximately)
DELETE FROM `is_labels`;
/*!40000 ALTER TABLE `is_labels` DISABLE KEYS */;
INSERT INTO `is_labels` (`id`, `clinic`, `idf`, `label`) VALUES
	(1, 3, 'web_titel', 'Informačný portál Kliniky detskej chirurgie LF UK a DFNsP'),
	(3, 3, 'OddA', 'Oddelenie A'),
	(4, 4, '2dk_shifts_setup', 'Uvoľni pre ostatných'),
	(5, 4, '2dk_shifts_active', 'Urob aktivnym'),
	(6, 4, '2dk_shifts_edit', 'Editovať'),
	(7, 4, '2dk_web_titel', 'Informačný portál 2.Detskej Kliniky LFUK a DFNsP'),
	(8, 3, 'kdch_web_titel', 'Informačný portál Kliniky detskej chirurgie LF UK a DFNsP'),
	(9, 4, '2dk_shifts_print', 'Služby 2.Detskej kliniky LF UK a DFNsP'),
	(10, 4, '2dk_shifts_sign', 'Schválila<br> MUDr.A.Hlavatá, <br>primárka 2.DK LF UK a DFNsP'),
	(11, 3, 'kdch_hlasko_titel', 'Hlásenie službieb KDCH, DOrK a KPU');
/*!40000 ALTER TABLE `is_labels` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
