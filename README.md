# Software for image manipulation

This software uses the `Magick.NET` library to provide some image manipulation features.

Basic filters are:

* Blur
* High pass filter
* Saturation editor

Advanced filters are:

* Border finder
* Detail pusher

"Artistic" filters are:

* Posterize/posterize dark
* Vertical stripes
* Vertical stripes negative

## Structure of the software

There are three main components of the software

* Bytehttps://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images
* ImageIterators
* Filters

### ByteImage

`Magick.NET` allows to get the contents of a loaded image as an array of bytes. The `ReadOnlyByteImage` type serves that purpose: to manage an image stored as a byte array.

This class requires the image's width and height on construction, in order to allow the usage of the `GetColor(x, y)` method, which uses some arithmetic manipulations to convert x, y coordinates to array indices.

The type `ByteImage`, which extends `ReadOnlyByteImage`, provides the `SetPixel(x, y, color)` method, to manipulate the byte image. Once again, the x and y coordinates are converted to array indices using arithmetic operations based on the image's dimensions.

The reason for which the `ByteImage` types have been choosen over the types provided by the library revolve around performance. Working directly with bytes, implementing a thin layer of abstraction (the usage of coordinates instead of indices) proved significantly faster in terms of computation times.

### ImageIterators

The application of filters in image processing usually revolves around repeating some operation for each pixel of the input image. Image iterators provide the facilities to repeat some operation on every pixel, without worrying about the iteration itself.

Iterators can be obtained through `ImageIteratorFactory`, which allows the creation of sync/async iterators, as well as an "automatic" approach (the `CreateIterator()` method, which forwards the call to one of the other two method depending on internal configuration).

Interactions with iterators is performed exclusively through the `IImageIterator` interface.

The two provided iterators are the following:

1. `ImageIterator`: basic sync iterator. Calls the given application on every pixel, row by row, left to right
2. `DistributedImageIterator`: iterator that distributes the computation to 4 `ImageIterator` objects, which iterate on equal portions of the input image. Internally uses async mechanisms to parallelize the iterations. It is, obviously, much faster than its sync counterpart, but some filters (e.g.: `VerticalStripesLight`) require an ordered row-by-row iteration, which this implementation cannot provide

### Filters

The base `Filter` type manages a series of mechanisms:

* Invokes the overridable `OnApplication` method, invoked immediately before the start of the filter application
* Defines the abstract `Application` method, which is applied to every pixel of the given image
* Retrieves the appropriate iterator depending on the `forceSync` field, and then runs it, passing it the `Application` method

The sub-types only override the `Application` method (besides fields and properties related to information and configuration), which receives, on every call, the input https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images and the coordinates of the pixel on which it was called.

# Examples
In this section there are some example usages, taken from the `https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images` directory.

## Castle

Input image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle.jpg?raw=true?raw=true)

Image blurred with a radius of 2
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_blurred[x2].jpg?raw=true)

Outline computed from original and blurred
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_outline.jpg?raw=true)

Enhanced version computed from original and outline
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_enhanced.jpg?raw=true)

`Posterize` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_posterized.jpg?raw=true)

`VerticalStripesLight` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_striped.jpg?raw=true)

`VerticalStripesNegative` fitler, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Castle/castle_striped_neg.jpg?raw=true)


## Landscape

Input image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input.jpg?raw=true)

Image blurred with a radius of 2
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input_blurred[x2].jpg?raw=true)

Enhanced version computed from original and outline
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input_enhanced.jpg?raw=true)

Saturated version, applied to the enhanced picture
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input_saturated.jpg?raw=true)

`VerticalStripesLight` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input_striped.jpg?raw=true)

`VerticalStripesNegative` fitler, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Landscape/input_striped_neg.jpg?raw=true)

## Skyline

Input image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline.jpg?raw=true)

Image blurred with a radius of 2
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_blurred[x2].jpg?raw=true)

Outline computed from original and blurred
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_outline.jpg?raw=true)

Enhanced version computed from original and outline
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_enhanced.jpg?raw=true)

`Posterize` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_posterized.jpg?raw=true)

`VerticalStripesLight` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_striped.jpg?raw=true)

`VerticalStripesNegative` fitler, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Skyline/skyline_striped_neg.jpg?raw=true)

## Tevere

Input image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere.jpg?raw=true)

Image blurred with a radius of 2
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_blurred[x2].jpg?raw=true)

Outline computed from original and blurred
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_outline.jpg?raw=true)

Enhanced version computed from original and outline
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_enhanced.jpg?raw=true)

`Posterize` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_posterized.jpg?raw=true)

`VerticalStripesLight` filter, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_striped.jpg?raw=true)

`VerticalStripesNegative` fitler, applied to the enhanced image
![](https://github.com/Alessandro-Di-Cicco/ImageManipulator/blob/main/Images/Tevere/tevere_striped_neg.jpg?raw=true)
