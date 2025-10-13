import React, { useState } from 'react';
import Hero from 'views/user/home/components/Hero';
import Cards from 'views/user/home/components/Cards';
import StartYourJourney from 'views/user/home/components/StartYourJourney';

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
    </>
  );
}

export default Home;
