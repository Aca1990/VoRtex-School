-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Nov 15, 2022 at 10:12 PM
-- Server version: 5.7.24-log
-- PHP Version: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `useraccess`
--

-- --------------------------------------------------------

--
-- Table structure for table `achievements`
--

CREATE TABLE `achievements` (
  `achievement_id` int(11) NOT NULL,
  `name` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `hostingaddresses`
--

CREATE TABLE `hostingaddresses` (
  `hosting_id` int(11) NOT NULL,
  `user_id` int(2) NOT NULL,
  `address` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `hostingaddresses`
--

INSERT INTO `hostingaddresses` (`hosting_id`, `user_id`, `address`) VALUES
(1, 3, '192.168.1.4');

-- --------------------------------------------------------

--
-- Table structure for table `lesson_environment`
--

CREATE TABLE `lesson_environment` (
  `lesson_environment_id` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `lesson_environment`
--

INSERT INTO `lesson_environment` (`lesson_environment_id`, `Name`) VALUES
(1, 'Classroom'),
(2, 'City'),
(3, 'Park');

-- --------------------------------------------------------

--
-- Table structure for table `microlessons`
--

CREATE TABLE `microlessons` (
  `microlesson_id` int(11) NOT NULL,
  `lesson_name` varchar(20) NOT NULL,
  `presentation_ppt_content` varchar(100) DEFAULT NULL,
  `user_id` int(11) NOT NULL,
  `lesson_environment_id` int(11) NOT NULL,
  `study_group_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `microlessons`
--

INSERT INTO `microlessons` (`microlesson_id`, `lesson_name`, `presentation_ppt_content`, `user_id`, `lesson_environment_id`, `study_group_id`) VALUES
(1, 'virtual_reality', NULL, 3, 2, 1),
(4, 'Test', NULL, 2, 1, 1),
(5, 'Test10', NULL, 3, 2, 1),
(6, 'Test111', NULL, 3, 1, 1),
(7, 'Test10_1', NULL, 3, 1, 1),
(8, 'Test11156', NULL, 3, 1, 1),
(9, 'Test19', NULL, 3, 2, 2),
(10, 'T1234', NULL, 3, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `role_id` int(2) NOT NULL,
  `role_name` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`role_id`, `role_name`) VALUES
(1, 'Professor'),
(2, 'Student');

-- --------------------------------------------------------

--
-- Table structure for table `study_group`
--

CREATE TABLE `study_group` (
  `study_group_id` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `study_group`
--

INSERT INTO `study_group` (`study_group_id`, `Name`) VALUES
(1, 'Computer Science'),
(2, 'Geografy');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(10) NOT NULL,
  `username` varchar(20) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `achievements` varchar(100) DEFAULT NULL,
  `face_recognition_image` varchar(100) DEFAULT NULL,
  `role_id` int(11) NOT NULL,
  `webplatform_username` varchar(20) DEFAULT NULL,
  `microlesson_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `hash`, `salt`, `achievements`, `face_recognition_image`, `role_id`, `webplatform_username`, `microlesson_id`) VALUES
(1, 'aca12345678', '$5$rounds=5000$somethingaca1234$TmUXSCsML6s5S9QujzRwZBt.jcl5vT.IzxNTH1O/38D', '$5$rounds=5000$somethingaca12345678$', 'ProffesorVortexChat', '', 2, 'aleksandar_jov', 1),
(2, 'aca87654321', '$5$rounds=5000$somethingaca8765$6AqFWVYEAhSGvzyez2SPMqQQpFzcGMui2HkA6GQXvdB', '$5$rounds=5000$somethingaca87654321$', 'ProffesorVortexChat,InteractWithUsers', '', 2, NULL, 1),
(3, 'acauser123', '$5$rounds=5000$somethingacauser$Y56RQ0QmeCwsMWIWUPVarkMTbDXtdKsaHw4ZNlgHQaD', '$5$rounds=5000$somethingacauser123$', 'ProffesorVortexChat,InteractWithUsers', 'user_images\\acauser123.jpg', 1, 'aleksandar_jovanovic', 1),
(4, 'acauser321', '$5$rounds=5000$somethingacauser$Y56RQ0QmeCwsMWIWUPVarkMTbDXtdKsaHw4ZNlgHQaD', '$5$rounds=5000$somethingacauser321$', 'InteractWithUsers', NULL, 2, NULL, 1),
(5, 'acauser111', '$5$rounds=5000$somethingacauser$Y56RQ0QmeCwsMWIWUPVarkMTbDXtdKsaHw4ZNlgHQaD', '$5$rounds=5000$somethingacauser111$', NULL, NULL, 2, NULL, NULL),
(6, 'sandra123', '$5$rounds=5000$somethingsandra1$d/SW8YQdznt3Q.PiApOWODnroGJnVLZr4T7uwZjcQ36', '$5$rounds=5000$somethingsandra123$', ',InteractWithUsers', NULL, 2, NULL, NULL),
(7, 'testuser1', '$5$rounds=5000$somethingtestuse$NXMVt8QOrU7Nl7PWIVaIMWYarFes8YqC./qG9ns5H89', '$5$rounds=5000$somethingtestuser1$', NULL, NULL, 2, NULL, NULL),
(8, 'milijana123', '$5$rounds=5000$somethingmilijan$u4R2VID1Z8n./LCUAwOxiII.XxhCqD2y9f1Ibcjo854', '$5$rounds=5000$somethingmilijana123$', NULL, NULL, 2, NULL, NULL),
(9, 'Milica123', '$5$rounds=5000$somethingMilica1$OhVf7MzRl4PfOZ9Q8dFMFnAOrblGb8UWyuQl2rbeRq.', '$5$rounds=5000$somethingMilica123$', NULL, NULL, 2, NULL, NULL),
(10, 'acaacaaca', '$5$rounds=5000$somethingacaacaa$MTzpjR57Kz6ZRsqUcY8Xk.QK314MqyEuRhsMd1m3R64', '$5$rounds=5000$somethingacaacaaca$', NULL, NULL, 2, NULL, NULL),
(11, '', '$5$rounds=5000$something$GBquX92fUdXEFvPC8EjDeaYS6JhG.XuehkA/JwRrBH6', '$5$rounds=5000$something$', NULL, NULL, 2, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `user_data`
--

CREATE TABLE `user_data` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `user_role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `web_platform_users`
--

CREATE TABLE `web_platform_users` (
  `id` int(11) NOT NULL,
  `name` int(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `achievements`
--
ALTER TABLE `achievements`
  ADD PRIMARY KEY (`achievement_id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Indexes for table `hostingaddresses`
--
ALTER TABLE `hostingaddresses`
  ADD PRIMARY KEY (`hosting_id`),
  ADD UNIQUE KEY `address` (`address`);

--
-- Indexes for table `lesson_environment`
--
ALTER TABLE `lesson_environment`
  ADD PRIMARY KEY (`lesson_environment_id`);

--
-- Indexes for table `microlessons`
--
ALTER TABLE `microlessons`
  ADD PRIMARY KEY (`microlesson_id`),
  ADD UNIQUE KEY `lesson_name` (`lesson_name`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `presentation_ppt_content` (`presentation_ppt_content`) USING BTREE,
  ADD KEY `study_group_id` (`study_group_id`) USING BTREE,
  ADD KEY `lesson_environment_id` (`lesson_environment_id`) USING BTREE;

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`role_id`);

--
-- Indexes for table `study_group`
--
ALTER TABLE `study_group`
  ADD PRIMARY KEY (`study_group_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `webplatform_username` (`webplatform_username`),
  ADD KEY `role_id` (`role_id`),
  ADD KEY `microlesson_id` (`microlesson_id`);

--
-- Indexes for table `user_data`
--
ALTER TABLE `user_data`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `web_platform_users`
--
ALTER TABLE `web_platform_users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `achievements`
--
ALTER TABLE `achievements`
  MODIFY `achievement_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `hostingaddresses`
--
ALTER TABLE `hostingaddresses`
  MODIFY `hosting_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `lesson_environment`
--
ALTER TABLE `lesson_environment`
  MODIFY `lesson_environment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `microlessons`
--
ALTER TABLE `microlessons`
  MODIFY `microlesson_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `role_id` int(2) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `study_group`
--
ALTER TABLE `study_group`
  MODIFY `study_group_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `user_data`
--
ALTER TABLE `user_data`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `web_platform_users`
--
ALTER TABLE `web_platform_users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `microlessons`
--
ALTER TABLE `microlessons`
  ADD CONSTRAINT `microlessons_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
