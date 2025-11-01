import React from 'react';
import { Box, Image, Text } from '@chakra-ui/react';
import Slider from 'react-slick';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';

export default function InteriorSlider({ photos = [] }) {
  if (!photos || photos.length === 0) return null;

  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    adaptiveHeight: true,
    autoplay: false,
  };

  return (
    <Box mt={16} userSelect="none">
      {/* 🧹 Xóa toàn bộ border/outline focus/active */}
      <style>
        {`
          .slick-slide,
          .slick-slide * {
            outline: none !important;
            box-shadow: none !important;
            border: none !important;
          }

          .slick-slide:focus,
          .slick-slide:active,
          .slick-slide img:focus,
          .slick-slide img:active,
          .slick-prev:focus,
          .slick-prev:active,
          .slick-next:focus,
          .slick-next:active,
          .slick-dots li button:focus,
          .slick-dots li button:active {
            outline: none !important;
            border: none !important;
            box-shadow: none !important;
          }

          /* Ngăn Chrome thêm border khi bấm chuột */
          button::-moz-focus-inner {
            border: 0 !important;
          }

          /* Ngăn Safari / Edge thêm highlight khi click */
          button:focus-visible {
            outline: none !important;
          }

          /* Ẩn focus ring trên slide wrapper */
          .slick-slider,
          .slick-list,
          .slick-track {
            outline: none !important;
            box-shadow: none !important;
            border: none !important;
          }
        `}
      </style>

      <Text fontSize="2xl" fontWeight="700" mb={6}>
        Vehicle Gallery
      </Text>

      <Slider {...settings}>
        {photos.map((photo, idx) => (
          <Box key={idx} px={2}>
            <Image
              src={photo.photoUrl}
              alt={`Interior view ${idx + 1}`}
              borderRadius="lg"
              mx="auto"
              h={{ base: '280px', md: '500px' }}
              w="100%"
              objectFit="cover"
              draggable="false"
              _focus={{ outline: 'none', boxShadow: 'none', border: 'none' }}
              _active={{ outline: 'none', boxShadow: 'none', border: 'none' }}
              onMouseDown={(e) => e.preventDefault()} // 🧩 chặn highlight khi click giữ
              onError={(e) => (e.target.style.display = 'none')}
            />
          </Box>
        ))}
      </Slider>
    </Box>
  );
}
