import React from 'react';
import { Box, useColorModeValue } from '@chakra-ui/react';
import HeroSection from './components/HeroSection';
import AboutSection from './components/AboutSection';
import FeaturesSection from './components/FeaturesSection';
import PromotionSection from './components/PromotionSection';
import SubscribeSection from './components/SubscribeSection';
import Sale from 'assets/img/home/sale.png';
import Gift from 'assets/img/home/gift.png';
import Discount from 'assets/img/home/discount.png';
import { FaGift, FaTags } from 'react-icons/fa';

export default function Home() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const brandColor = useColorModeValue('brand.500', 'brand.400');
  const bgColor = useColorModeValue('white', 'navy.800');

  return (
    <Box pt={{ base: '80px', md: '100px' }} pb="60px">
      <HeroSection textColor={textColor} />
      <AboutSection />
      <FeaturesSection bgColor={bgColor} brandColor={brandColor} />
      <PromotionSection
        bgColor={bgColor}
        image={Sale}
        title="Grand Opening Sale 🎉"
        desc="Enjoy up to 10% OFF on all items this week only. Don’t miss out!"
        buttonText="Grab the Deal"
        colorScheme="brand"
        brandColor={brandColor}
        textColor={textColor}
      />
      <PromotionSection
        bgColor={bgColor}
        image={Gift}
        title="Special Gifts 🎁"
        desc="Get exclusive gifts with selected purchases. Limited stock available!"
        buttonText="Claim Gift"
        buttonIcon={<FaGift />}
        colorScheme="pink"
        reverse
        brandColor={brandColor}
        textColor={textColor}
      />
      <PromotionSection
        bgColor={bgColor}
        image={Discount}
        title="Weekend Flash Sale ⚡"
        desc="Enjoy amazing discounts on top categories. Hurry up, time is running out!"
        buttonText="Shop Deals"
        buttonIcon={<FaTags />}
        colorScheme="yellow"
        brandColor={brandColor}
        textColor={textColor}
      />
      <SubscribeSection bgColor={bgColor} textColor={textColor} />
    </Box>
  );
}
