# A Virtual Reality-Based Assessment System for Tremor Disorders Using CNN and RNN for Comprehensive Symptom Analysis 

## 0. Overview
This project explores the use of Virtual Reality (VR) platforms combined with IMU sensors to diagnose movement disorders, specifically focusing on conditions such as Parkinson's disease and essential tremor. 
Current diagnostic tools, such as the Unified Parkinson's Disease Rating Scale (UPDRS) and the TETRAS scale for essential tremor, rely on subjective assessments, often leading to potential biases in diagnosis.
[Thesis for the Master of Science](https://www.riss.kr/search/detail/DetailView.do?p_mat_type=be54d9b8bc7cdb09&control_no=a6318027226bf546ffe0bdc3ef48d419)

## 1. Pictures

### Tasks(Spooning And Drawing)

###### Two Tasks - Spooning(Transfering Tomatos) and Drawing a spiral.
<img src="https://github.com/user-attachments/assets/069d922c-08e6-4958-b31a-45bf1e6ef64f"  width="500" height="400"/>
<img src="https://github.com/user-attachments/assets/b0209c91-db1a-4afa-8281-6a8d4bedd9d6"  width="500" height="400"/>




### 2. Custom-designed Board and Modeling

###### PCB Modeling
<img src="https://github.com/user-attachments/assets/2d9a14d4-bffe-410f-ab8f-fb90dc72cccf"  width="400" height="200"/>
<img src="https://github.com/user-attachments/assets/f30fbfd0-fa15-4a32-ab15-6bb347987dab"  width="400" height="200"/>

###### Spoon and Marker
<img src="https://github.com/user-attachments/assets/ed2d272f-3ab6-48ab-9f5c-c56f41bcef15"  width="400" height="200"/>
<img src="https://github.com/user-attachments/assets/13a00e2b-0f96-4cdf-be1e-5a24fe4b593e"  width="400" height="200"/>

###### Controllers
<img src="https://github.com/user-attachments/assets/a937555f-ae0d-4b20-a329-fa59d28b7fe3"  width="400" height="200"/>


## 3. GIFs

![드로잉 외부화면](https://github.com/user-attachments/assets/d3f06cf1-34f2-4895-b02a-524e93016199)
![숟가락 외부화면](https://github.com/user-attachments/assets/c1faf098-1795-4022-8661-51ab8086b05f)

## 4. Breif Explanation

###### Task Design
<img src="https://github.com/user-attachments/assets/e18f91a1-ca39-4b19-bb52-ee962f147ad4"  width="500" height="600"/>

In the VR environment implemented with a Head Mounted Display (HMD), participants used devices equipped with IMU sensors and the nRF52840 microcontroller to simulate daily activities. Each of the two tasks was divided into five levels of difficulty. For the first task, transferring food from one bowl to another, the difficulty was increased by extending the distance between the bowls. In the second task, drawing a spiral pattern on a blackboard, the difficulty was adjusted by increasing the size of the spiral.

While the participants performed these tasks, 3-axis acceleration and orientation data detected by the IMU sensors, along with the position of the tool in virtual reality, were recorded. The collected data was then used for statistical analysis based on Fitts' law, with the difficulty level for each task considered in the analysis.

Additionally, we are currently conducting research to analyze the data using AI-based analysis techniques (e.g., CNN, RNN), beyond simple statistical analysis.

### 5. Data Analysis Examples

###### Table
<img src="https://github.com/user-attachments/assets/70e40289-20d0-4806-a627-4c692e02c17a"  width="500" height="250"/>

###### CNN
<img src="https://github.com/user-attachments/assets/f49e6ea5-8de1-4461-b9a2-a3331852ee4a"  width="400" height="200"/>

###### RNN
<img src="https://github.com/user-attachments/assets/9540dc7f-ca2c-4712-95a5-38e2cdf23fad"  width="400" height="200"/>

## License Information

This project is licensed for **personal use only**. Any academic or commercial use is strictly prohibited without the explicit permission of the author.
For more details, please refer to the [LICENSE](./LICENSE) file.
If you wish to use this software for academic or commercial purposes, please contact the author at [cbcc12345@hanyang.ac.kr] or [cbcc1234@gmail.com].
Unauthorized use may result in legal action.
