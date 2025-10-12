import { Image } from '@chakra-ui/react';
import React from 'react';

function Picture({ styl }) {
  return (
    <picture>
      <source
        srcSet="https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.0d0cc89@1x.webp 1x,
                 https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.2245c45@2x.webp 2x,
                 https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.19b4292@3x.webp 3x"
        type="image/webp"
      />
      <source
        srcSet="https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.d76137c@1x.png 1x,
                 https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.8a292fb@2x.png 2x,
                 https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.18d6f02@3x.png 3x"
        type="image/png"
      />
      <Image
        src="https://cdn.ui.porsche.com/porsche-design-system/crest/porsche-crest.8a292fb@2x.png"
        alt="Porsche"
        w="30px"
        h="40px"
        filter={styl ? 'invert(1)' : 'none'}
      />
    </picture>
  );
}

export default Picture;
