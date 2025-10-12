import React, { useEffect, useRef, useState } from 'react';
import Hls from 'hls.js';
import {
  Box,
  Heading,
  IconButton,
  Image,
  Flex,
  Button,
  useBreakpointValue,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { NavLink } from 'react-router-dom';

const MotionBox = motion(Box);

function Hero({ isCategoryOpen }) {
  const videoRef = useRef(null);
  const [isPaused, setIsPaused] = useState(false);
  const isDesktop = useBreakpointValue({ base: false, md: true });

  useEffect(() => {
    const video = videoRef.current;
    if (!video) return;

    if (Hls.isSupported()) {
      const hls = new Hls();
      const src = isDesktop
        ? 'https://videos.porsche.com/id/taycanbepc/hls.m3u8'
        : 'https://videos.porsche.com/id/taycanbemob/hls.m3u8';
      hls.loadSource(src);
      hls.attachMedia(video);

      const handlePause = () => setIsPaused(true);
      const handlePlay = () => setIsPaused(false);

      video.addEventListener('pause', handlePause);
      video.addEventListener('play', handlePlay);

      return () => {
        video.removeEventListener('pause', handlePause);
        video.removeEventListener('play', handlePlay);
        hls.destroy();
      };
    }
  }, [isDesktop]);

  const togglePlay = () => {
    const video = videoRef.current;
    if (!video) return;
    video.paused ? video.play() : video.pause();
  };

  return (
    <Box
      position="relative"
      minH="100vh"
      overflow="hidden"
      display="grid"
      gridTemplateRows={{
        base: 'auto 1fr auto',
        md: 'auto [headline] max-content [cta] 1fr',
      }}
      filter={isCategoryOpen ? 'blur(10px)' : 'none'}
      transition="filter 0.3s ease"
      _before={
        isCategoryOpen
          ? {
              content: '""',
              position: 'absolute',
              inset: 0,
              bg: 'rgba(0,0,0,0.5)',
              zIndex: 2,
            }
          : {}
      }
    >
      {/* 🎥 Background video */}
      <Box position="absolute" inset="0" zIndex={0}>
        <video
          ref={videoRef}
          autoPlay
          muted
          loop
          playsInline
          crossOrigin="anonymous"
          style={{
            width: '100%',
            height: '100%',
            objectFit: 'cover',
          }}
        />
      </Box>

      {/* 🌑 Gradient overlay */}
      <Box
        position="absolute"
        inset="0"
        bgGradient="linear(to-t, rgba(0,0,0,0.8), rgba(0,0,0,0))"
        zIndex={1}
      />

      <Box
        position="absolute"
        bottom={20}
        left={20}
        zIndex={3}
        px={{ base: 4, md: 16 }}
        pt={{ base: 24, md: 32 }}
      >
        {/* 🏁 Heading */}
        <MotionBox
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1.2, ease: 'easeOut' }}
        >
          <Heading
            color="white"
            fontSize={{ base: '3xl', md: '6xl', lg: '7xl' }}
            fontWeight="medium"
            lineHeight="1.1"
            w="70%"
          >
            Soul in every detail.
          </Heading>
        </MotionBox>

        {/* 🚘 Discover Button (replaces GotoButtons) */}
        <MotionBox
          zIndex={3}
          mt={{ base: 8, md: 12 }}
          initial={{ opacity: 0, y: 40 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 1, delay: 0.5 }}
        >
          <Button
            as={NavLink}
            to="/model/taycan-4-black-edition"
            variant="outline"
            borderColor="white"
            color="white"
            borderWidth="2px"
            borderRadius="md"
            size="lg"
            px={{ base: 6, md: 10 }}
            py={6}
            fonWeight="400"
            fontFamily="body"
            bg="transparent"
            _hover={{
              bg: 'whiteAlpha.200',
              borderColor: 'whiteAlpha.900',
            }}
            transition="all 0.3s ease"
          >
            Discover more
          </Button>
        </MotionBox>
      </Box>

      {/* ▶️ Play / Pause Button */}
      <Flex
        position="absolute"
        bottom={{ base: 6, md: 12 }}
        right={{ base: 6, md: 16 }}
        zIndex={4}
      >
        <IconButton
          onClick={togglePlay}
          aria-label="Play or Pause"
          variant="outline"
          border="2px solid"
          borderColor="whiteAlpha.700"
          borderRadius="md"
          bg="transparent"
          p={3}
          _hover={{ bg: 'whiteAlpha.300' }}
          icon={
            <Image
              src={
                isPaused
                  ? 'https://cdn.ui.porsche.com/porsche-design-system/icons/play.24226d4.svg'
                  : 'https://cdn.ui.porsche.com/porsche-design-system/icons/pause.e41b935.svg'
              }
              w="24px"
              h="24px"
              filter="invert(100%) brightness(120%)"
            />
          }
        />
      </Flex>
    </Box>
  );
}

export default Hero;
