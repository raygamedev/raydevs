import { motion } from 'framer-motion';
import { Unity, useUnityContext } from 'react-unity-webgl';
import { useEffect } from 'react';

interface RaydevsGameProps {
  setIsGameLoaded: (value: boolean) => void;
  isPlaying: boolean;
}
export const RaydevsGame = ({
  setIsGameLoaded,
  isPlaying,
}: RaydevsGameProps) => {
  const { unityProvider, isLoaded } = useUnityContext({
    loaderUrl: 'build/WebGL.loader.js',
    dataUrl: 'build/WebGL.data',
    frameworkUrl: 'build/WebGL.framework.js',
    codeUrl: 'build/WebGL.wasm',
  });

  // some commit
  useEffect(() => setIsGameLoaded(isLoaded), [setIsGameLoaded, isLoaded]);
  return (
    <motion.div
      hidden={!isPlaying}
      transition={{ type: 'spring', duration: 0.8 }}
      animate={{
        x: 0,
        y: isPlaying ? 0 : 2000,
        opacity: isPlaying ? 1 : 0,
        rotate: 0,
      }}
      style={{
        position: 'absolute',
        width: '100%',
        height: '100%',
        borderRadius: '15px',
      }}>
      <Unity
        style={{
          width: '100%',
          height: '100%',
          borderRadius: '15px',
        }}
        unityProvider={unityProvider}
      />
    </motion.div>
  );
};
