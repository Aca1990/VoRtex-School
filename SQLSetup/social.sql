-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Host: sql213.epizy.com
-- Generation Time: Jan 24, 2023 at 06:30 AM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 7.2.22

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `social`
--

-- --------------------------------------------------------

--
-- Table structure for table `comments`
--

CREATE TABLE `comments` (
  `id` int(11) NOT NULL,
  `post_body` text NOT NULL,
  `posted_by` varchar(60) NOT NULL,
  `posted_to` varchar(60) NOT NULL,
  `date_added` datetime NOT NULL,
  `removed` varchar(3) NOT NULL,
  `post_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `friend_requests`
--

CREATE TABLE `friend_requests` (
  `id` int(11) NOT NULL,
  `user_to` varchar(50) NOT NULL,
  `user_from` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `likes`
--

CREATE TABLE `likes` (
  `id` int(11) NOT NULL,
  `username` varchar(60) NOT NULL,
  `post_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE `messages` (
  `id` int(11) NOT NULL,
  `user_to` varchar(50) NOT NULL,
  `user_from` varchar(50) NOT NULL,
  `body` text NOT NULL,
  `date` datetime NOT NULL,
  `opened` varchar(3) NOT NULL,
  `viewed` varchar(3) NOT NULL,
  `deleted` varchar(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--

INSERT INTO `messages` (`id`, `user_to`, `user_from`, `body`, `date`, `opened`, `viewed`, `deleted`) VALUES
(1, 'aleksandar_jovanovic', 'aleksandar_jovanovic', 'gfgfgfgfg', '2022-11-19 22:50:07', 'yes', 'yes', 'no');

-- --------------------------------------------------------

--
-- Table structure for table `notifications`
--

CREATE TABLE `notifications` (
  `id` int(11) NOT NULL,
  `user_to` varchar(50) NOT NULL,
  `user_from` varchar(50) NOT NULL,
  `message` text NOT NULL,
  `link` varchar(100) NOT NULL,
  `datetime` datetime NOT NULL,
  `opened` varchar(3) NOT NULL,
  `viewed` varchar(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `posts`
--

CREATE TABLE `posts` (
  `id` int(11) NOT NULL,
  `body` text NOT NULL,
  `added_by` varchar(60) NOT NULL,
  `user_to` varchar(60) NOT NULL,
  `date_added` datetime NOT NULL,
  `user_closed` varchar(3) NOT NULL,
  `deleted` varchar(3) NOT NULL,
  `likes` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posts`
--

INSERT INTO `posts` (`id`, `body`, `added_by`, `user_to`, `date_added`, `user_closed`, `deleted`, `likes`) VALUES
(66, 'Aca', 'aleksandar_jovanovic', 'none', '2019-06-15 16:19:10', 'no', 'no', 2),
(67, 'Aacaada', 'aleksandar_jovanovic', 'none', '2022-11-17 12:48:35', 'no', 'no', 0),
(68, '1234', 'aleksandar_jovanovic', 'none', '2022-11-17 23:36:07', 'no', 'no', 0),
(69, 'Helooooo', 'anna_bryl', 'none', '2022-11-19 22:51:07', 'no', 'no', 0);

-- --------------------------------------------------------

--
-- Table structure for table `trends`
--

CREATE TABLE `trends` (
  `title` varchar(50) NOT NULL,
  `hits` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `first_name` varchar(25) NOT NULL,
  `last_name` varchar(25) NOT NULL,
  `username` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `signup_date` date NOT NULL,
  `profile_pic` varchar(255) NOT NULL,
  `num_posts` int(11) NOT NULL,
  `num_likes` int(11) NOT NULL,
  `user_closed` varchar(3) NOT NULL,
  `friend_array` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `first_name`, `last_name`, `username`, `email`, `password`, `signup_date`, `profile_pic`, `num_posts`, `num_likes`, `user_closed`, `friend_array`) VALUES
(11, 'Aleksandar', 'Jovanovic', 'aleksandar_jovanovic', 'Aca.jovanovic1990@gmail.com', 'b2d1bcdc698b0ff497ed80d68fd0b13c', '2019-06-15', 'assets/images/profile_pics/defaults/head_deep_blue.png', 5, 2, 'no', ','),
(12, 'Aleksandar', 'Jovanovic', 'aleksandar_jovanovic_1', 'Acke.jovanovic.90@gmail.com', 'b2d1bcdc698b0ff497ed80d68fd0b13c', '2020-03-22', 'assets/images/profile_pics/defaults/head_emerald.png', 0, 0, 'no', ','),
(13, 'Aleksandar', 'Jovanovic', 'aleksandar_jovanovic_1_2', 'Contact@remote-human.com', '0f6558cdffd0119dbecfcd5ef5614ffb', '2022-11-19', 'assets/images/profile_pics/defaults/head_emerald.png', 0, 0, 'no', ','),
(14, 'Anna', 'Bryl', 'anna_bryl', 'Annauser123@vortex.com', 'deff7e0ce47950b461d0a462402d3194', '2022-11-19', 'assets/images/profile_pics/defaults/head_emerald.png', 1, 0, 'no', ','),
(15, 'Aleksandar', 'Milosavljevic', 'aleksandar_milosavljevic', 'Amilosavljevic@vortex.com', '25d55ad283aa400af464c76d713c07ad', '2022-11-20', 'assets/images/profile_pics/defaults/head_deep_blue.png', 0, 0, 'no', ','),
(16, 'Aleksandar', 'Stanimirovic', 'aleksandar_stanimirovic', 'Astanimirovic@vortex.com', '5e8667a439c68f5145dd2fcbecf02209', '2022-11-20', 'assets/images/profile_pics/defaults/head_emerald.png', 0, 0, 'no', ','),
(17, 'Aleksandar', 'Dimitrijevic', 'aleksandar_dimitrijevic', 'Adimitrijevic@vortex.com', 'ea195efa08a2c8f1c40e2baf56d5afa0', '2022-11-20', 'assets/images/profile_pics/defaults/head_emerald.png', 0, 0, 'no', ','),
(18, 'Student', 'Student1', 'student_student1', 'Student1@vortex.com', 'ad6a280417a0f533d8b670c61667e1a0', '2022-11-20', 'assets/images/profile_pics/defaults/head_deep_blue.png', 0, 0, 'no', ',');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `comments`
--
ALTER TABLE `comments`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `friend_requests`
--
ALTER TABLE `friend_requests`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `likes`
--
ALTER TABLE `likes`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `messages`
--
ALTER TABLE `messages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `notifications`
--
ALTER TABLE `notifications`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `posts`
--
ALTER TABLE `posts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `comments`
--
ALTER TABLE `comments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `friend_requests`
--
ALTER TABLE `friend_requests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `likes`
--
ALTER TABLE `likes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `messages`
--
ALTER TABLE `messages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `notifications`
--
ALTER TABLE `notifications`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `posts`
--
ALTER TABLE `posts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=70;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
