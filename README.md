# A Virtual Reality-Based Assessment System for Tremor Disorders Using CNN and RNN for Comprehensive Symptom Analysis 

## 0. Overview
This project explores the use of Virtual Reality (VR) platforms combined with IMU sensors to diagnose movement disorders, specifically focusing on conditions such as Parkinson's disease and essential tremor. 
Current diagnostic tools, such as the Unified Parkinson's Disease Rating Scale (UPDRS) and the TETRAS scale for essential tremor, rely on subjective assessments, often leading to potential biases in diagnosis.
[Thesis for the Master of Science](https://www.riss.kr/search/detail/DetailView.do?p_mat_type=be54d9b8bc7cdb09&control_no=a6318027226bf546ffe0bdc3ef48d419)

## 1. Pictures

### Tasks(Spooning And Drawing)

![Spooning](https://github.com/user-attachments/assets/069d922c-08e6-4958-b31a-45bf1e6ef64f)
![Drawing](https://github.com/user-attachments/assets/b0209c91-db1a-4afa-8281-6a8d4bedd9d6)

### 2. Custom-designed Board and Modeling

![Custom-designed Board](https://github.com/user-attachments/assets/2d9a14d4-bffe-410f-ab8f-fb90dc72cccf)
![image](https://github.com/user-attachments/assets/8cd3c28d-abbb-4962-8cdd-d0971f97c1ae)

![Spoon](https://github.com/user-attachments/assets/c1b4d9aa-9c8f-45c1-8631-5044df10b772)
![Spooon](https://github.com/user-attachments/assets/ed2d272f-3ab6-48ab-9f5c-c56f41bcef15)
![Marker](https://github.com/user-attachments/assets/13a00e2b-0f96-4cdf-be1e-5a24fe4b593e)

## 3. Videos

https://github.com/user-attachments/assets/d83c568d-5c40-49ca-bca4-efd353bf4da3
https://github.com/user-attachments/assets/743ca44c-4326-4589-9b2b-2f57e76d7bc5

## 4. Breif Explanation

 ![그림2](https://github.com/user-attachments/assets/b3b7e8fe-f19a-4fbe-ac0e-a6439e69ce64)
In the VR environment implemented with a Head Mounted Display (HMD), participants used devices equipped with IMU sensors and the nRF52840 microcontroller to simulate daily activities. Each of the two tasks was divided into five levels of difficulty. For the first task, transferring food from one bowl to another, the difficulty was increased by extending the distance between the bowls. In the second task, drawing a spiral pattern on a blackboard, the difficulty was adjusted by increasing the size of the spiral.

While the participants performed these tasks, 3-axis acceleration and orientation data detected by the IMU sensors, along with the position of the tool in virtual reality, were recorded. The collected data was then used for statistical analysis based on Fitts' law, with the difficulty level for each task considered in the analysis.

Additionally, we are currently conducting research to analyze the data using AI-based analysis techniques (e.g., CNN, RNN), beyond simple statistical analysis.

### 5. Analysis Example

![Graph1](https://github.com/user-attachments/assets/6136bcea-f0a0-4e9c-add3-a1bc84aa65eb)
![Graph2](https://github.com/user-attachments/assets/9125ad2f-10c6-4a30-adc2-c05c4b912036)



## License Information

This project is licensed for **personal use only**. Any academic or commercial use is strictly prohibited without the explicit permission of the author.
For more details, please refer to the [LICENSE](./LICENSE) file.
If you wish to use this software for academic or commercial purposes, please contact the author at [cbcc12345@hanyang.ac.kr] or [cbcc1234@gmail.com].
Unauthorized use may result in legal action.
