import numpy as np
import matplotlib.pyplot as plt
import numpy as np
from scipy.integrate import quad

# 매개변수 설정
w = 0   # 문제에서 주어진 w 값

# n을 2~4까지하면 각각 1바퀴~3바퀴
for n in range(2, 5):

    # 적분 범위 설정
    theta_start = 2 * np.pi
    theta_end = 2 * np.pi * (n + 1)


    # 매개변수
    w = 0

    # 각도 범위 설정
    theta = np.linspace(theta_start, theta_end, 10000) 

    # 나선의 반지름 계산
    def find_r(theta):
        r = (w + theta)**2
        return r
    # 극좌표를 직교좌표로 변환 함수
    def find_x(theta):
        x = find_r(theta) * np.cos(theta)
        return x

    def find_y(theta):
        y = find_r(theta) * np.sin(theta)
        return y


    #n바꾸 돌았을 때의 x값을 구하고 그걸 원점과 이은 점...
    def find_nx(plus):
        theta = theta_start + plus
        x = find_x(theta)
        return x

    # 적분 함수 정의
    def integrand(theta):
        # numerator = np.sqrt((theta + w)**6 + 9 * (theta + w)**4)
        # denominator = (theta + 2 * np.pi + w)**3 - (theta + w)**3
        numerator = np.sqrt((theta + w)**4 + 4 * (theta + w)**2)
        denominator = (theta + 2 * np.pi + w)**2 - (theta + w)**2
        return numerator / denominator

    # 수치적분 수행
    ID_value, error = quad(integrand, theta_start, theta_end)

    # 결과 출력
    print(f"Figure{n-1}의 ID 값: {ID_value}")

    x1 = np.linspace(find_nx(0),find_nx(theta_start),1000)
    y1 = [0 for _ in range(len(x1))]
    x2 = np.linspace(find_nx(theta_end-theta_start-2*np.pi),find_nx(theta_end-theta_start),1000)
    y2 = [0 for _ in range(len(x1))]


    # 나선을 그림
    fig = plt.figure(figsize=(8, 8))#, facecolor= 'black')
    fig.patch.set_alpha(0)
    plt.plot(find_x(theta), find_y(theta), color = 'red', linewidth = 3) #, label=r'$r = (\omega + \theta)^3$')
    plt.plot(x1,y1, color = 'darkorange', linestyle = 'dotted', linewidth = 4)
    plt.plot(x2,y2, color = 'darkorange', linestyle='dotted', linewidth = 4)

    # plt.title('아르키메데스 나선 예제')
    # plt.xlabel('x')
    # plt.ylabel('y')
    plt.axis('off')
    plt.axis('equal')
    # plt.xlim(-28000, 37000)
    # plt.ylim(-36400, 28300)
    plt.xlim(-891, 1080)
    plt.ylim(-1063, 885)
    plt.grid(False)
    plt.savefig('Figure' + str(n-1) + '.png',format = 'png')
    plt.show()


