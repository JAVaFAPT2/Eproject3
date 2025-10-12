import React, { useState } from 'react';
import Hero from 'views/user/home/components/Hero';
import Cards from 'views/user/home/components/Cards';
import StartYourJourney from 'views/user/home/components/StartYourJourney';
import Discover from 'views/user/home/components/Discover';

function Home() {
  const [isCategoryOpen, setIsCategoryOpen] = useState(false);
  return (
    <>
      <Hero
        isCategoryOpen={isCategoryOpen}
        setIsCategoryOpen={setIsCategoryOpen}
      />
      <Cards />
      <StartYourJourney />
      <Discover />
    </>
  );
}

export default Home;
