using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponController : MonoBehaviour
{
    //Possible and current weapon variables
    int curWeapon;

    //Animation
    public SpriteRenderer rend;
    public Rigidbody2D playerBod;
    //recoil for player
    public float force;
    //knockback for enemies
    public float knockback = 10f;
    int weaponId = 0;

    //Bullets
    public GameObject[] spawns;
    public GameObject bullet;
    public GameObject shell;
    public GameObject smgBullet;
    public GameObject rocket;
    public GameObject shotgunShell;
    public int bulAmt;
    [HideInInspector]
    public List<GameObject> bulList;
    [HideInInspector]
    public List<GameObject> shellList;
    [HideInInspector]
    public List<GameObject> shotgunShellList;
    [HideInInspector]
    public List<GameObject> smgBulletList;
    [HideInInspector]
    public List<GameObject> rocketList;

    //Rotation
    Vector3 mousePos;
    public float lerpSpd;
    public float uiLerpSpd;

    //Accuracy
    public float accuracyLow;
    public float accuracyHigh;
    float accuracyMid;
    float curAcc;
    public float aimLerp;
    public float accMod = 0f;

    //Player actions/Input
    PlayerController player;
    //PlayerActions playerActions;
    //float shot;
    //float aim;
    //float run;
    //float mouseScroll;

    //Shooting
    float cools = 0f;
    float cooldown = 0.15f;
    float damage;

    //Reticle
    public GameObject reticle;
    public Vector3 reticleScale;
    float clamp;

    //Animations
    Animator anim;

    //Starting weapon
    public Items startingItems;

    //Inventory
    public int maxSlots;
    int curSlot = 0;
    int filledSlots = 0;
    public Image[] hotbarBox;
    public Image[] reloadHotbar;
    public Image[] hotbarSprites;
    public List<Items> playerItems = new List<Items>();
    public int[] hotbarSlotsAmmo;
    float dmgLow;
    float dmgHigh;
    int shellCount = 0;

    public Equipment curEquipment;
    public int equipUses = 0;

    //Reloading
    float reloadTime;
    bool reloading = false;
    bool canReload;

    //Current ammo capacity
    int pistolAmmo;
    int shotgunAmmo;
    int machinegunAmmo;
    int sniperAmmo;
    int rpgAmmo;
    int flamethrowerAmmo;

    //Max ammo capacity
    public int maxPistolAmmo = 70;
    public int maxShotgunAmmo = 40;
    public int maxSniperAmmo = 30;
    public int maxMachinegunAmmo = 120;
    public int maxRpgAmmo = 5;
    public int maxFlamethrowerAmmo;
    
    //ammo UI
    public Image clipImage;
    //public Text clipText;
    public TextMeshProUGUI clipText;

    //reload UI
    public Image reloadCircleImage;
    public Image reloadBarImage;
    public Image reloadBarImageBG;

    public SniperController sniper;
    public LineRenderer laserSight;

    //Flamethrower
    public FlamethrowerController flames;
    public Collider2D flameTrigger;

    float curTypeAmmo = 0;

    public float zombarCooldownTime;
    float zombarCooldown;
    float dmgMod = 0;
    public ScriptableFloat scriptDmg;

    //Player can't move or shoot whilst in menus
    public bool canShoot = true;

    public Animator flashObj;
    public Animator playerAnim;

    //Audio
    AudioSource src;
    public AudioClip pistolShoot;
    public AudioClip sniperShoot;
    public AudioClip mgShoot;
    public AudioClip shotgunShoot;
    public AudioClip rpgShoot;
    public AudioClip sgClick;
    public AudioClip reloadClip;
    public AudioClip singleReload;

    public Image equipImage;
    public TMP_Text equipText;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        player = FindObjectOfType<PlayerController>();
        //playerActions = new PlayerActions();
        //playerActions.PlayerControls.Aim.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        //playerActions.PlayerControls.Shoot.performed += ctx => shot = ctx.ReadValue<float>();
        //playerActions.PlayerControls.RMB.performed += ctx => aim = ctx.ReadValue<float>();
        //playerActions.PlayerControls.Run.performed += ctx => run = ctx.ReadValue<float>();
        //playerActions.PlayerControls.MouseScroll.performed += ctx => mouseScroll = ctx.ReadValue<float>();

        hotbarSlotsAmmo = new int[5];

        reticleScale = reticle.transform.localScale;

        accuracyMid = (accuracyLow + accuracyHigh) / 2;
        curAcc = accuracyMid;

        src = GetComponent<AudioSource>();
        //rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        bulList = new List<GameObject>();
        for (int i = 0; i < bulAmt; i++)
        {
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
            obj.SetActive(false);
            bulList.Add(obj);
        }

        if (startingItems != null) AddItem(startingItems);

        FillAmmo();

        if (curEquipment != null) RefillEquipment();

        laserSight.enabled = false;
    }

    public void InMenu()
    {
        canShoot = false;
    }

    public void ExitMenu()
    {
        canShoot = true;
    }

    public void SetZombar()
    {
        zombarCooldown = zombarCooldownTime;
    }

    public void FillAmmo()
    {
        pistolAmmo = maxPistolAmmo;
        shotgunAmmo = maxShotgunAmmo;
        sniperAmmo = maxSniperAmmo;
        rpgAmmo = maxRpgAmmo;
        machinegunAmmo = maxMachinegunAmmo;
        flamethrowerAmmo = maxFlamethrowerAmmo;
    }

    GameObject GetBullet()
    {
        //Return hit obj
        for (int i = 0; i < bulList.Count; i++)
        {
            if (!bulList[i].activeInHierarchy)
            {
                return bulList[i];
            }
        }

        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        bulList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    GameObject GetSMGBullet()
    {
        if (smgBulletList.Count <= 0)
        {
            GameObject o = Instantiate(smgBullet, transform.position, Quaternion.identity);
            smgBulletList.Add(o);
            o.SetActive(false);

            return o;
        }
        //Return hit obj
        for (int i = 0; i < smgBulletList.Count; i++)
        {
            if (!smgBulletList[i].activeInHierarchy)
            {
                return smgBulletList[i];
            }
        }

        GameObject obj = Instantiate(smgBullet, transform.position, Quaternion.identity);
        smgBulletList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    GameObject GetRocket()
    {
        if (rocketList.Count <= 0)
        {
            GameObject o = Instantiate(rocket, transform.position, Quaternion.identity);
            rocketList.Add(o);
            o.SetActive(false);

            return o;
        }
        //Return hit obj
        for (int i = 0; i < rocketList.Count; i++)
        {
            if (!rocketList[i].activeInHierarchy)
            {
                return rocketList[i];
            }
        }

        GameObject obj = Instantiate(rocket, transform.position, Quaternion.identity);
        rocketList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    GameObject GetShot()
    {
        //Return hit obj
        for (int i = 0; i < shellList.Count; i++)
        {
            if (!shellList[i].activeInHierarchy)
            {
                return shellList[i];
            }
        }

        GameObject obj = Instantiate(shell, transform.position, Quaternion.identity);
        shellList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    GameObject GetShell()
    {
        //Return hit obj
        for (int i = 0; i < shotgunShellList.Count; i++)
        {
            if (!shotgunShellList[i].activeInHierarchy)
            {
                return shotgunShellList[i];
            }
        }

        GameObject obj = Instantiate(shotgunShell, transform.position, Quaternion.identity);
        shotgunShellList.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    private void Update()
    {
        //Aim at mouse
        //Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        //transform.right = direction;

        //bool myBool = Mathf.Approximately(aim, 1);

        if (zombarCooldown > 0)
        {
            dmgMod = scriptDmg.val;
            zombarCooldown -= Time.deltaTime;
        }
        else
        {
            dmgMod = 0;
        }

        //Firing
        if (Input.GetMouseButton(0) && cools <= 0f && canShoot)
        {
            //Check for if theres still ammo in the current clip
            if (hotbarSlotsAmmo[curSlot] > 0)
            {
                //anim.SetBool("Attacking", true);
                switch (curWeapon)
                {
                    case ((int)Items.types.pistol):
                        FirePistol();
                        break;
                    case ((int)Items.types.machinegun):
                        FireSMG();
                        break;
                    case ((int)Items.types.shotgun):
                        FireShotgunShot();
                        break;
                    case ((int)Items.types.sword):

                        break;
                    case ((int)Items.types.flamethrower):
                        FireFlamethrower();
                        break;
                    case ((int)Items.types.rpg):
                        FireRocket();
                        break;
                    case ((int)Items.types.sniper):
                        FireSniper();
                        break;
                }
                reloading = false;
                hotbarSlotsAmmo[curSlot]--;
            }
            else
            {
                //anim.SetBool("Attacking", false);
                if (!reloading && canReload)
                {
                    //Reload when you try to shoot with an empty clip
                    reloadHotbar[curSlot].fillAmount = 0f;
                    reloading = true;
                }

                if (curWeapon == (int)Items.types.flamethrower)
                {

                    flames.enabled = false;
                    flameTrigger.enabled = false;
                }
            }
        }

        //Use Equipment
        if (Input.GetButtonDown("Fire2") && equipUses > 0 && curEquipment != null)
        {
            curEquipment.UseItem();
            //curEquipment.fills--;
            equipUses--;
            equipText.text = "x" + equipUses.ToString();
        }

        if (Input.GetButtonUp("Fire1") && curWeapon == (int)Items.types.flamethrower)
        {
            flames.enabled = false;
            flameTrigger.enabled = false;
        }

        //anim.SetInteger("curWeapon", weaponId);

        if (Input.GetButton("Run") && player.curStam > 0)
        {
            //Running
            curAcc = Mathf.Lerp(curAcc, accuracyHigh, aimLerp * Time.deltaTime);
        }
        else if (Input.GetButton("Aim"))
        {
            curAcc = Mathf.Lerp(curAcc, accuracyLow, aimLerp * Time.deltaTime);
        }
        else
        {
            //Not aiming
            curAcc = Mathf.Lerp(curAcc, accuracyMid, aimLerp * Time.deltaTime);
        }

        //Switching weapons with the hotbar
        if (Input.GetButtonDown("Switch"))
        {
            if (curSlot > 0)
            {
                //SwitchItem(curSlot - 1);
                SwitchItem((curSlot + 1) % filledSlots);
            }
            else
            {
                if (filledSlots > 1)
                {
                    //SwitchItem(filledSlots - 1);
                    SwitchItem((curSlot + 1) % filledSlots);
                }
            }
        }

        //Reloading
        if (Input.GetButtonDown("Reload") && hotbarSlotsAmmo[curSlot] < playerItems[curSlot].clipSize && !reloading && canReload)
        {
            reloadHotbar[curSlot].fillAmount = 0f;
            
            if (playerItems[curSlot].singleReload) src.PlayOneShot(singleReload);
            else src.PlayOneShot(reloadClip);
            
            reloading = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (filledSlots > 1)
            {
                SwitchItem(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (filledSlots > 2)
            {
                SwitchItem(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (filledSlots > 3)
            {
                SwitchItem(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (filledSlots > 4)
            {
                SwitchItem(4);
            }
        }

        if (cools > 0) cools -= Time.deltaTime;
        clipText.text = hotbarSlotsAmmo[curSlot].ToString() + " / " + curTypeAmmo.ToString(); ;

        //Reloading
        if (reloading) reloadTime += Time.deltaTime;
        else reloadTime = 0f;
        //Switch case for different reload types
        if (reloadTime >= playerItems[curSlot].reloadTime)
        {
            Reload(playerItems[curSlot]);
        }
        //Countdown for reloading
        if (reloading) reloadCircleImage.fillAmount = (reloadTime / playerItems[curSlot].reloadTime);
        else reloadCircleImage.fillAmount = 0f;

        //Reloading UI
        if (reloading)
        {
            reloadHotbar[curSlot].fillAmount = Mathf.Lerp(reloadHotbar[curSlot].fillAmount, (reloadTime / playerItems[curSlot].reloadTime), Time.deltaTime * uiLerpSpd);
            //reloadBarImageBG.color = Color.black;
            //reloadCircleImage.fillAmount = Mathf.Lerp(reloadCircleImage.fillAmount, (reloadTime / playerItems[curSlot].reloadTime), Time.deltaTime * uiLerpSpd);
            //reloadBarImage.fillAmount = Mathf.Lerp(reloadBarImage.fillAmount, (reloadTime / playerItems[curSlot].reloadTime), Time.deltaTime * uiLerpSpd);
        }
        else
        {
            reloadHotbar[curSlot].fillAmount = 1f;
            //reloadBarImageBG.color = Color.clear;
            //reloadCircleImage.fillAmount = 0f;
            //reloadBarImage.fillAmount = 0f;
        }

        //See if you can reload
        //Also update current ammotype
        switch (curWeapon)
        {
            case ((int)Items.types.pistol):
                curTypeAmmo = pistolAmmo;
                canReload = (pistolAmmo > 0);
                break;
            case ((int)Items.types.machinegun):
                canReload = (machinegunAmmo > 0);
                curTypeAmmo = machinegunAmmo;
                break;
            case ((int)Items.types.shotgun):
                canReload = (shotgunAmmo > 0);
                curTypeAmmo = shotgunAmmo;
                break;
            case ((int)Items.types.flamethrower):
                canReload = (flamethrowerAmmo > 0);
                curTypeAmmo = flamethrowerAmmo;
                break;
            case ((int)Items.types.sniper):
                curTypeAmmo = sniperAmmo;
                canReload = (sniperAmmo > 0);
                break;
            case ((int)Items.types.rpg):
                canReload = (rpgAmmo > 0);
                curTypeAmmo = rpgAmmo;
                break;
        }
    }
    Vector3 lookPos;

    private void LateUpdate()
    {
        //Sprite position
        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        lookPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Show weapon in front of or behind player
        rend.sortingOrder = (lookPos.y < 0.5f) ? 3 : 1;
        rend.flipY = (lookPos.x < 0.5f) ? true : false;

        //Animate player
        playerAnim.SetFloat("moveY", lookPos.y);

        //Look at mouse
        //Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 dir = mousePos - Camera.main.WorldToScreenPoint(transform.position);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * lerpSpd);

        Vector3 newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        reticle.transform.position = new Vector3(newMousePos.x, newMousePos.y, 0);
        clamp = curAcc / 2.5f;
        clamp = Mathf.Clamp(clamp, 1f, 2.5f);
        reticle.transform.localScale = Vector3.Lerp(reticle.transform.localScale, reticleScale * clamp, lerpSpd * Time.deltaTime);

        //anim.SetBool("Attacking", false);
        //anim.SetInteger("curWeapon", curSlot);
    }

    public bool needsAmmo(Items.types ty)
    {
        //Check to see if ammo is full for certain type
        switch(ty)
        {
            case (Items.types.pistol):
                if (pistolAmmo < maxPistolAmmo) return true;
                break;
            case (Items.types.machinegun):
                if (machinegunAmmo < maxMachinegunAmmo) return true;
                break;
            case (Items.types.shotgun):
                if (shotgunAmmo < maxShotgunAmmo) return true;
                break;
            case (Items.types.sniper):
                if (sniperAmmo < maxSniperAmmo) return true;
                break;
            case (Items.types.rpg):
                if (rpgAmmo < maxRpgAmmo) return true;
                break;
            case (Items.types.flamethrower):
                if (flamethrowerAmmo < maxFlamethrowerAmmo) return true;
                break;
        }

        return false;
    }

    public bool needsAnyAmmo()
    {
        if (pistolAmmo < maxPistolAmmo || machinegunAmmo < maxMachinegunAmmo || shotgunAmmo < maxShotgunAmmo | sniperAmmo < maxSniperAmmo
            | rpgAmmo < maxRpgAmmo | flamethrowerAmmo < maxFlamethrowerAmmo) return true;

        return false;
    }

    public void RefillAmmo(Items.types ty)
    {
        switch ((int)ty)
        {
            case ((int)Items.types.pistol):
                pistolAmmo = maxPistolAmmo;
                break;
            case ((int)Items.types.machinegun):
                machinegunAmmo = maxMachinegunAmmo;
                break;
            case ((int)Items.types.shotgun):
                shotgunAmmo = maxShotgunAmmo;
                break;
            case ((int)Items.types.sniper):
                sniperAmmo = maxSniperAmmo;
                break;
            case ((int)Items.types.flamethrower):
                flamethrowerAmmo = maxFlamethrowerAmmo;
                break;
            case ((int)Items.types.rpg):
                rpgAmmo = maxRpgAmmo;
                break;
        }
    }

    public bool hasWeapon(Items it)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].name == it.name) return true;
        }
        return false;
    }

    public bool hasEquipment(Equipment it)
    {
        if (it.name == curEquipment.name)
        {
            return true;
        }
        else return false;
    }

    public bool hasRoom()
    {
        if (filledSlots <= maxSlots - 1) return true;
        return false;
    }

    public void SwitchEquipment(Equipment it)
    {
        curEquipment = it;
        RefillEquipment();

        //Gotta change the ui and count here too
        equipImage.sprite = it.sprite;
        //Size of equipment image
        //equipImage
        equipText.text = "x" + equipUses.ToString();
    }

    public void RefillEquipment()
    {
        equipUses = curEquipment.fills;
        equipText.text = "x" + equipUses.ToString();
        equipImage.sprite = curEquipment.sprite;
    }

    public void AddItem(Items item)
    {
        if (hasRoom())
        {
            playerItems.Add(item);
            hotbarSprites[filledSlots].rectTransform.sizeDelta = new Vector2(playerItems[filledSlots].spriteWidth, playerItems[filledSlots].spriteHeight);
            hotbarSprites[filledSlots].sprite = item.GetComponent<SpriteRenderer>().sprite;
            hotbarSprites[filledSlots].color = Color.white;
            hotbarSlotsAmmo[filledSlots] = item.clipSize;
        }
        else
        {
            playerItems.Remove(playerItems[curSlot]);
            playerItems.Insert(curSlot, item);
            hotbarSprites[curSlot].rectTransform.sizeDelta = new Vector2(playerItems[curSlot].spriteWidth, playerItems[curSlot].spriteHeight);
            hotbarSprites[filledSlots].sprite = item.GetComponent<SpriteRenderer>().sprite;
            hotbarSprites[curSlot].color = Color.white;
            hotbarSlotsAmmo[curSlot] = item.clipSize;
            SwitchItem(curSlot);
            return;
        }

        if (filledSlots == 0)
        {
            SwitchItem(0);
            //rend.sprite = playerItems[0].sprite;
        }
        filledSlots++;
        //Debug.Log("Filled: " + filledSlots + "\nMax slots: " + maxSlots);
    }

    public void SwitchItem(int slot)
    {
        //flameParticleSys.SetActive(false);
        reloadHotbar[curSlot].fillAmount = 0f;
        curSlot = slot;
        curWeapon = (int)playerItems[curSlot].thisType;
        cooldown = playerItems[curSlot].cooldown;
        accuracyHigh = playerItems[curSlot].spreadHigh + accMod;
        accuracyLow = playerItems[curSlot].spreadLow + accMod;
        if (accuracyLow + accMod < 0f) accuracyLow = 0f;
        accuracyMid = (accuracyLow + accuracyHigh) / 2 + accMod;
        force = playerItems[curSlot].knockback;
        weaponId = playerItems[curSlot].id;
        clipImage.sprite = playerItems[curSlot].ammoImage;
        dmgLow = playerItems[curSlot].dmgLow;
        dmgHigh = playerItems[curSlot].dmgHigh;
        reloading = false;
        shellCount = playerItems[curSlot].shellCount;
        if (curWeapon == (int)Items.types.sniper)
        {
            laserSight.enabled = true;
            sniper.dmgLow = dmgLow;
            sniper.dmgHigh = dmgHigh;
        }
        else
        {
            laserSight.enabled = false;
        }
        //Set weapon sprite
        rend.sprite = playerItems[curSlot].sprite;
        //Set flash location
        spawns[0].transform.localPosition = new Vector2(rend.sprite.bounds.max.x, transform.localPosition.y);
    }

    void Reload(Items itemToReload)
    {
        if (itemToReload.singleReload)
        {
            reloadTime = 0f;
            hotbarSlotsAmmo[curSlot]++;
            switch (curWeapon)
            {
                case ((int)Items.types.pistol):
                    pistolAmmo--;
                    break;
                case ((int)Items.types.machinegun):
                    machinegunAmmo--;
                    break;
                case ((int)Items.types.shotgun):
                    shotgunAmmo--;
                    break;
                case ((int)Items.types.flamethrower):
                    flamethrowerAmmo--;
                    break;
                case ((int)Items.types.rpg):
                    rpgAmmo--;
                    break;
                case ((int)Items.types.sniper):
                    sniperAmmo--;
                    break;
            }

            //src.PlayOneShot(singleReload);

            if (hotbarSlotsAmmo[curSlot] == playerItems[curSlot].clipSize)
            {
                reloading = false;
            }
            else src.PlayOneShot(singleReload);
        }
        else
        {
            switch (curWeapon)
            {
                case ((int)Items.types.pistol):
                    pistolAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (pistolAmmo <= 0) pistolAmmo = 0;
                    break;
                case ((int)Items.types.machinegun):
                    machinegunAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (machinegunAmmo <= 0) machinegunAmmo = 0;
                    break;
                case ((int)Items.types.shotgun):
                    shotgunAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (shotgunAmmo <= 0) shotgunAmmo = 0;
                    break;
                case ((int)Items.types.flamethrower):
                    flamethrowerAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (flamethrowerAmmo <= 0) flamethrowerAmmo = 0;
                    break;
                case ((int)Items.types.rpg):
                    rpgAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (rpgAmmo <= 0) rpgAmmo = 0;
                    break;
                case ((int)Items.types.sniper):
                    sniperAmmo -= (playerItems[curSlot].clipSize - hotbarSlotsAmmo[curSlot]);
                    if (sniperAmmo <= 0) sniperAmmo = 0;
                    break;
            }

            //src.PlayOneShot(reloadClip);

            hotbarSlotsAmmo[curSlot] = playerItems[curSlot].clipSize;
            reloading = false;
        }

        //Debug.Log("Pistol ammo: " + pistolAmmo);
        //Debug.Log("Machinegun ammo: " + machinegunAmmo);
        //Debug.Log("Shotgun ammo: " + shotgunAmmo);
        //Debug.Log("Flamethrower ammo: " + flamethrowerAmmo);
        //Debug.Log("RPG ammo: " + rpgAmmo);
    }

    void FirePistol()
    {
        float ran = Random.Range(-curAcc, curAcc);
        GameObject obj = GetBullet();
        obj.GetComponent<BulletController>().SetDamage(dmgLow, dmgHigh, dmgMod);
        obj.transform.position = spawns[0].transform.position;
        playerBod.AddForceAtPosition(-transform.up * force, transform.position);
        obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, ran);
        cools = cooldown;
        obj.SetActive(true);

        src.PlayOneShot(pistolShoot);

        SetFlash();
    }
    
    void SetFlash()
    {
        //Random rotation
        flashObj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        //Play anim
        flashObj.SetTrigger("Flash");
    }

    void FireSMG()
    {
        float ran = Random.Range(-curAcc, curAcc);
        GameObject obj = GetSMGBullet();
        obj.GetComponent<BulletController>().SetDamage(dmgLow, dmgHigh, dmgMod);
        obj.transform.position = spawns[0].transform.position;
        playerBod.AddForceAtPosition(-transform.up * force, transform.position);
        obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, ran);
        cools = cooldown;
        obj.SetActive(true);
        src.PlayOneShot(mgShoot);

        SetFlash();
    }

    public void FireSniper()
    {
        //Also might want some other sprite for this. Smoke trail maybe?
        //GameObject flash = GetFlashObj();
        //flash.transform.position = spawns[0].transform.position;
        //flash.SetActive(true);

        //reloadWheel.fillAmount = 0;
        sniper.StartLaserCoroutine();
        cools = cooldown;

        //sniper.laserCools = cooldown;

        src.PlayOneShot(sniperShoot);

        SetFlash();
    }

    void FireRocket()
    {
        float ran = Random.Range(-curAcc, curAcc);
        GameObject obj = GetRocket();
        //obj.GetComponent<BulletController>().SetDamage(dmgLow, dmgHigh, dmgMod);
        obj.transform.position = spawns[0].transform.position;
        playerBod.AddForceAtPosition(-transform.up * force, transform.position);
        obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, ran);
        cools = cooldown;
        obj.SetActive(true);
        src.PlayOneShot(rpgShoot);

        SetFlash();
    }

    //Default shotgun that I've always done
    /*void FireShotgun()
    {
        float ran = Random.Range(-curAcc, curAcc);
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = GetBullet();
            obj.GetComponent<BulletController>().SetDamage(dmgLow, dmgHigh);
            obj.transform.position = spawns[0].transform.position;
            playerBod.AddForceAtPosition(-transform.right * force, transform.position);
            obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -90 + (i * curAcc) - curAcc);
            cools = cooldown;
            obj.SetActive(true);
        }
    }*/

    //Shotgun shooting with 'shots' that shoot out in the same direction
    void FireShotgunShot()
    {
        float ran = Random.Range(-curAcc, curAcc);
        GameObject shell = GetShell();
        shell.transform.position = (transform.position + spawns[0].transform.position) / 2;
        shell.SetActive(true);
        for (int i = 0; i < shellCount; i++)
        {
            GameObject obj = GetShot();
            obj.GetComponent<BulletController>().SetDamage(dmgLow, dmgHigh, dmgMod);
            obj.transform.position = (Vector2)spawns[0].transform.position + Random.insideUnitCircle * curAcc * (0.035f + Random.Range(-0.035f, 0.035f));
            playerBod.AddForceAtPosition(-transform.up * force, transform.position);
            obj.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, (i * curAcc) - curAcc);
            obj.SetActive(true);
        }
        //Play audio clip for weapon
        src.PlayOneShot(shotgunShoot);
        if (playerItems[curSlot].cocks) Invoke("PlayShotgunClick", shotgunShoot.length - 0.1f);
        cools = sgClick.length + 0.025f;
        //Set muzzle flash animation/sprite
        SetFlash();
        //Set cooldown
        cools = cooldown;
    }

    void PlayShotgunClick()
    {
        src.PlayOneShot(sgClick);
    }

    void FireFlamethrower()
    {
        flames.enabled = true;
        flameTrigger.enabled = true;
    }
}
